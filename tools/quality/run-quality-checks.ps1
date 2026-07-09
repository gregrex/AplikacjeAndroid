param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [switch]$SyncRuntimeFirst
)

$ErrorActionPreference = "Stop"

Write-Host "AppFactory quality checks"
Write-Host "Repo root: $RepoRoot"

if ($SyncRuntimeFirst) {
    $syncScript = Join-Path $PSScriptRoot "sync-runtime-packs.ps1"
    if (-not (Test-Path $syncScript)) {
        throw "Sync script not found: $syncScript"
    }

    Write-Host "Synchronizing runtime packs before tests..."
    & $syncScript -RepoRoot $RepoRoot
}

$testProject = Join-Path $RepoRoot "tests\AppFactory.Mobile.Tests\AppFactory.Mobile.Tests.csproj"
if (-not (Test-Path $testProject)) {
    throw "Test project not found: $testProject"
}

Write-Host "Running dotnet test for project quality..."
& dotnet test $testProject --verbosity minimal

if ($LASTEXITCODE -ne 0) {
    Write-Host "Quality checks failed. Read the test output above and fix the first failing project/file."
    exit $LASTEXITCODE
}

Write-Host "Quality checks completed successfully."
