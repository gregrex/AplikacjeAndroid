param(
    [string]$PackageName = "pl.gbcom.appfactory",
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [int]$LogcatLines = 3000,
    [switch]$CreateZip
)

$ErrorActionPreference = "Stop"

if (-not (Get-Command adb -ErrorAction SilentlyContinue)) {
    throw "adb command was not found in PATH."
}

$state = (& adb get-state 2>$null | Out-String).Trim()
if ($state -ne "device") {
    throw "No Android device or emulator is connected. Run: adb devices"
}

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$outputRoot = Join-Path $RepoRoot "artifacts\device-diagnostics\$timestamp"
$appLogsRoot = Join-Path $outputRoot "app-logs"
New-Item -ItemType Directory -Force -Path $appLogsRoot | Out-Null

Write-Host "Collecting diagnostics from package: $PackageName"
Write-Host "Output: $outputRoot"

$manifest = [ordered]@{
    collectedAt = (Get-Date).ToString("o")
    packageName = $PackageName
    adbSerial = (& adb get-serialno | Out-String).Trim()
    manufacturer = (& adb shell getprop ro.product.manufacturer | Out-String).Trim()
    model = (& adb shell getprop ro.product.model | Out-String).Trim()
    androidVersion = (& adb shell getprop ro.build.version.release | Out-String).Trim()
    sdk = (& adb shell getprop ro.build.version.sdk | Out-String).Trim()
    buildFingerprint = (& adb shell getprop ro.build.fingerprint | Out-String).Trim()
}

$manifest | ConvertTo-Json -Depth 4 | Set-Content -Path (Join-Path $outputRoot "device-manifest.json") -Encoding UTF8

& adb devices -l | Set-Content -Path (Join-Path $outputRoot "adb-devices.txt") -Encoding UTF8
& adb shell dumpsys package $PackageName | Set-Content -Path (Join-Path $outputRoot "package-dumpsys.txt") -Encoding UTF8
& adb logcat -d -v threadtime -t $LogcatLines | Set-Content -Path (Join-Path $outputRoot "logcat.txt") -Encoding UTF8

$remoteListOutput = & adb shell run-as $PackageName ls files/logs 2>&1
if ($LASTEXITCODE -ne 0) {
    $remoteListOutput | Set-Content -Path (Join-Path $outputRoot "app-logs-error.txt") -Encoding UTF8
    Write-Warning "Could not read private app logs with run-as. Use a Debug build, verify the package name, and ensure the application has been started at least once."
}
else {
    $remoteFiles = $remoteListOutput |
        ForEach-Object { $_.ToString().Trim() } |
        Where-Object { $_ -match '\.jsonl$' }

    foreach ($fileName in $remoteFiles) {
        if ($fileName -notmatch '^[a-zA-Z0-9._-]+$') {
            Write-Warning "Skipping unexpected remote file name: $fileName"
            continue
        }

        $destination = Join-Path $appLogsRoot $fileName
        $process = Start-Process `
            -FilePath "adb" `
            -ArgumentList @("exec-out", "run-as", $PackageName, "cat", "files/logs/$fileName") `
            -NoNewWindow `
            -Wait `
            -PassThru `
            -RedirectStandardOutput $destination `
            -RedirectStandardError (Join-Path $outputRoot "$fileName.stderr.txt")

        if ($process.ExitCode -ne 0) {
            Write-Warning "Failed to pull $fileName (exit code $($process.ExitCode))."
        }
        else {
            Write-Host "Pulled: $fileName"
        }
    }
}

$summary = New-Object System.Collections.Generic.List[string]
$summary.Add("# Android diagnostics collection")
$summary.Add("")
$summary.Add("- Collected: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')")
$summary.Add("- Package: `$PackageName`")
$summary.Add("- Device: $($manifest.manufacturer) $($manifest.model)")
$summary.Add("- Android: $($manifest.androidVersion) / SDK $($manifest.sdk)")
$summary.Add("- App log files: $((Get-ChildItem $appLogsRoot -Filter '*.jsonl' -ErrorAction SilentlyContinue).Count)")
$summary.Add("- Logcat lines requested: $LogcatLines")
$summary.Add("")
$summary.Add("Attach this directory to a defect only after reviewing it for sensitive information.")
$summary | Set-Content -Path (Join-Path $outputRoot "summary.md") -Encoding UTF8

if ($CreateZip) {
    $zipPath = "$outputRoot.zip"
    if (Test-Path $zipPath) {
        Remove-Item $zipPath -Force
    }
    Compress-Archive -Path (Join-Path $outputRoot "*") -DestinationPath $zipPath -CompressionLevel Optimal
    Write-Host "ZIP: $zipPath"
}

Write-Host "Diagnostics collected: $outputRoot"
