param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [switch]$RestoreWorkloads,
    [switch]$IncludeReleaseBuild,
    [switch]$WriteReport,
    [switch]$SyncRuntime,
    [switch]$SkipAndroidBuild
)

$ErrorActionPreference = "Stop"

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$sessionStarted = Get-Date
$resultsRoot = Join-Path $RepoRoot "artifacts\local-test\$timestamp"
$summaryPath = Join-Path $resultsRoot "summary.md"
$testProject = Join-Path $RepoRoot "tests\AppFactory.Mobile.Tests\AppFactory.Mobile.Tests.csproj"
$mobileProject = Join-Path $RepoRoot "src\AppFactory.Mobile\AppFactory.Mobile.csproj"
$summaryRows = New-Object System.Collections.Generic.List[string]

New-Item -ItemType Directory -Force -Path $resultsRoot | Out-Null

function Write-Summary {
    $lines = New-Object System.Collections.Generic.List[string]
    $lines.Add("# AppFactory local test summary")
    $lines.Add("")
    $lines.Add("- Started: $($sessionStarted.ToString('yyyy-MM-dd HH:mm:ss'))")
    $lines.Add("- Updated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')")
    $lines.Add("- Repository: `$RepoRoot`")
    $lines.Add("- Results: `$resultsRoot`")
    $lines.Add("")
    $lines.Add("| Step | Status | Duration | Log |")
    $lines.Add("| --- | --- | ---: | --- |")
    foreach ($row in $summaryRows) {
        $lines.Add($row)
    }
    $lines.Add("")
    $lines.Add("## Manual next steps")
    $lines.Add("")
    $lines.Add("1. Run the Android smoke test from `docs/quality/LOCAL_TEST_PLAN.md`.")
    $lines.Add("2. Update `docs/quality/SCENARIO_EXECUTION_TRACKER.md`.")
    $lines.Add("3. Record defects for every FAIL or BLOCKED scenario.")
    Set-Content -Path $summaryPath -Value $lines -Encoding UTF8
}

function Invoke-NativeStep {
    param(
        [string]$Name,
        [string]$FilePath,
        [string[]]$Arguments
    )

    $safeName = ($Name -replace '[^a-zA-Z0-9_-]', '-')
    $stdoutPath = Join-Path $resultsRoot "$safeName.stdout.log"
    $stderrPath = Join-Path $resultsRoot "$safeName.stderr.log"
    $started = Get-Date

    Write-Host ""
    Write-Host "=== $Name ==="
    Write-Host "$FilePath $($Arguments -join ' ')"

    try {
        $process = Start-Process `
            -FilePath $FilePath `
            -ArgumentList $Arguments `
            -WorkingDirectory $RepoRoot `
            -NoNewWindow `
            -Wait `
            -PassThru `
            -RedirectStandardOutput $stdoutPath `
            -RedirectStandardError $stderrPath

        if (Test-Path $stdoutPath) {
            Get-Content $stdoutPath | Write-Host
        }
        if (Test-Path $stderrPath) {
            Get-Content $stderrPath | Write-Host
        }

        $duration = [Math]::Round(((Get-Date) - $started).TotalSeconds, 1)
        if ($process.ExitCode -ne 0) {
            $summaryRows.Add("| $Name | FAIL ($($process.ExitCode)) | ${duration}s | `$safeName.stdout.log`, `$safeName.stderr.log` |")
            Write-Summary
            throw "Step '$Name' failed with exit code $($process.ExitCode)."
        }

        $summaryRows.Add("| $Name | PASS | ${duration}s | `$safeName.stdout.log`, `$safeName.stderr.log` |")
    }
    catch {
        if (-not ($summaryRows | Where-Object { $_ -like "| $Name |*" })) {
            $duration = [Math]::Round(((Get-Date) - $started).TotalSeconds, 1)
            $summaryRows.Add("| $Name | FAIL | ${duration}s | `$safeName.stdout.log`, `$safeName.stderr.log` |")
        }
        Write-Summary
        throw
    }
}

function Add-SkippedStep {
    param([string]$Name, [string]$Reason)
    $summaryRows.Add("| $Name | SKIPPED | - | $Reason |")
}

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw "dotnet command was not found in PATH."
}

if (-not (Test-Path $testProject)) {
    throw "Test project not found: $testProject"
}

if (-not (Test-Path $mobileProject)) {
    throw "Mobile project not found: $mobileProject"
}

Invoke-NativeStep -Name "dotnet-info" -FilePath "dotnet" -Arguments @("--info")
Invoke-NativeStep -Name "workload-list" -FilePath "dotnet" -Arguments @("workload", "list")

if ($RestoreWorkloads) {
    Invoke-NativeStep -Name "workload-restore" -FilePath "dotnet" -Arguments @("workload", "restore", $mobileProject)
}
else {
    Add-SkippedStep -Name "workload-restore" -Reason "Use -RestoreWorkloads to enable."
}

if ($SyncRuntime) {
    $syncScript = Join-Path $PSScriptRoot "sync-runtime-packs.ps1"
    Invoke-NativeStep -Name "runtime-sync" -FilePath "pwsh" -Arguments @("-NoProfile", "-File", $syncScript, "-RepoRoot", $RepoRoot)
}
else {
    Add-SkippedStep -Name "runtime-sync" -Reason "Use -SyncRuntime only after reviewing source/runtime differences."
}

Invoke-NativeStep -Name "restore-tests" -FilePath "dotnet" -Arguments @("restore", $testProject)
Invoke-NativeStep -Name "restore-mobile" -FilePath "dotnet" -Arguments @("restore", $mobileProject)

Invoke-NativeStep -Name "tests-all" -FilePath "dotnet" -Arguments @(
    "test", $testProject,
    "-c", "Release",
    "--no-restore",
    "--logger", "trx;LogFileName=tests.trx",
    "--results-directory", $resultsRoot,
    "--verbosity", "minimal"
)

Invoke-NativeStep -Name "tests-sqlite" -FilePath "dotnet" -Arguments @(
    "test", $testProject,
    "-c", "Release",
    "--no-restore",
    "--filter", "FullyQualifiedName~AppDatabaseTests|FullyQualifiedName~LocalDatabaseProductionTests",
    "--logger", "trx;LogFileName=sqlite-tests.trx",
    "--results-directory", $resultsRoot,
    "--verbosity", "minimal"
)

if (-not $SkipAndroidBuild) {
    Invoke-NativeStep -Name "android-debug-build" -FilePath "dotnet" -Arguments @(
        "build", $mobileProject,
        "-f", "net9.0-android",
        "-c", "Debug",
        "--no-restore",
        "--verbosity", "minimal"
    )

    if ($IncludeReleaseBuild) {
        Invoke-NativeStep -Name "android-release-build" -FilePath "dotnet" -Arguments @(
            "build", $mobileProject,
            "-f", "net9.0-android",
            "-c", "Release",
            "--no-restore",
            "--verbosity", "minimal"
        )
    }
    else {
        Add-SkippedStep -Name "android-release-build" -Reason "Use -IncludeReleaseBuild to enable."
    }
}
else {
    Add-SkippedStep -Name "android-debug-build" -Reason "Disabled with -SkipAndroidBuild."
    Add-SkippedStep -Name "android-release-build" -Reason "Disabled with -SkipAndroidBuild."
}

if ($WriteReport) {
    $reportScript = Join-Path $PSScriptRoot "write-project-quality-report.ps1"
    Invoke-NativeStep -Name "quality-report" -FilePath "pwsh" -Arguments @("-NoProfile", "-File", $reportScript, "-RepoRoot", $RepoRoot)
}
else {
    Add-SkippedStep -Name "quality-report" -Reason "Use -WriteReport to enable."
}

if (Get-Command adb -ErrorAction SilentlyContinue) {
    Invoke-NativeStep -Name "adb-devices" -FilePath "adb" -Arguments @("devices", "-l")
}
else {
    Add-SkippedStep -Name "adb-devices" -Reason "adb is not available in PATH."
}

Write-Summary
Write-Host ""
Write-Host "Local test automation completed."
Write-Host "Summary: $summaryPath"
