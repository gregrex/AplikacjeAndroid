param(
    [Parameter(Mandatory = $true)][ValidatePattern('^[0-9]{2}-[a-z0-9-]+$')][string]$Name,
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [string]$OutputDirectory = (Join-Path $RepoRoot "artifacts\google-play\screenshots\raw")
)

$ErrorActionPreference = "Stop"

if (-not (Get-Command adb -ErrorAction SilentlyContinue)) {
    throw "adb was not found in PATH."
}

$state = (& adb get-state 2>$null | Out-String).Trim()
if ($state -ne "device") {
    throw "No Android device or emulator is connected. Run: adb devices"
}

New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$outputPath = Join-Path $OutputDirectory "$Name.png"

$startInfo = [System.Diagnostics.ProcessStartInfo]::new()
$startInfo.FileName = "adb"
$startInfo.ArgumentList.Add("exec-out")
$startInfo.ArgumentList.Add("screencap")
$startInfo.ArgumentList.Add("-p")
$startInfo.UseShellExecute = $false
$startInfo.RedirectStandardOutput = $true
$startInfo.RedirectStandardError = $true
$startInfo.CreateNoWindow = $true

$process = [System.Diagnostics.Process]::new()
$process.StartInfo = $startInfo
if (-not $process.Start()) {
    throw "Could not start adb."
}

$file = [System.IO.File]::Create($outputPath)
try {
    $process.StandardOutput.BaseStream.CopyTo($file)
}
finally {
    $file.Dispose()
}

$errorText = $process.StandardError.ReadToEnd()
$process.WaitForExit()
if ($process.ExitCode -ne 0) {
    Remove-Item $outputPath -Force -ErrorAction SilentlyContinue
    throw "adb screencap failed: $errorText"
}

$info = Get-Item $outputPath
if ($info.Length -lt 1024) {
    throw "Captured file is unexpectedly small: $($info.Length) bytes"
}

if ($IsWindows) {
    Add-Type -AssemblyName System.Drawing.Common
    $image = [System.Drawing.Image]::FromFile($outputPath)
    try {
        Write-Host "Captured dimensions: $($image.Width)x$($image.Height)"
        if ($image.Height -le $image.Width) {
            Write-Warning "Google Play phone screenshot should normally be portrait."
        }
    }
    finally {
        $image.Dispose()
    }
}

Write-Host "Screenshot saved: $outputPath"
Write-Host "Review it for private data, debug controls, Local AI panels and stale UI before approval."
