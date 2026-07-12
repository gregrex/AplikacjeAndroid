param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [switch]$SkipTests,
    [switch]$SkipAndroidBuild,
    [switch]$RequireScreenshots,
    [string]$KeystorePath,
    [string]$KeyAlias = "appfactory-upload",
    [string]$KeyPasswordFile,
    [string]$StorePasswordFile
)

$ErrorActionPreference = "Stop"

$testProject = Join-Path $RepoRoot "tests\AppFactory.Mobile.Tests\AppFactory.Mobile.Tests.csproj"
$mobileProject = Join-Path $RepoRoot "src\AppFactory.Mobile\AppFactory.Mobile.csproj"
$resultsRoot = Join-Path $RepoRoot "artifacts\google-play"
$summaryPath = Join-Path $resultsRoot "package-summary.md"
$steps = New-Object System.Collections.Generic.List[string]

New-Item -ItemType Directory -Force -Path $resultsRoot | Out-Null

function Add-Step([string]$Name, [string]$Status, [string]$Details) {
    $steps.Add("| $Name | $Status | $Details |")
}

function Invoke-Step {
    param([string]$Name, [scriptblock]$Action)
    Write-Host ""
    Write-Host "=== $Name ==="
    try {
        & $Action
        Add-Step $Name "PASS" ""
    }
    catch {
        Add-Step $Name "FAIL" ($_.Exception.Message -replace '\|', '/')
        Write-Summary
        throw
    }
}

function Write-Summary {
    $lines = New-Object System.Collections.Generic.List[string]
    $lines.Add("# Google Play package summary")
    $lines.Add("")
    $lines.Add("Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')")
    $lines.Add("")
    $lines.Add("| Step | Status | Details |")
    $lines.Add("| --- | --- | --- |")
    foreach ($step in $steps) { $lines.Add($step) }
    $lines.Add("")
    $lines.Add("## Owner actions still required")
    $lines.Add("")
    $lines.Add("- Enable GitHub Pages and verify public URLs.")
    $lines.Add("- Review legal declarations in Play Console.")
    $lines.Add("- Create and protect the upload key.")
    $lines.Add("- Capture and approve six screenshots from the final build.")
    $lines.Add("- Upload the signed AAB to Internal testing.")
    $lines.Add("- Complete required closed testing and production access steps.")
    Set-Content $summaryPath $lines -Encoding UTF8
}

if (-not (Get-Command pwsh -ErrorAction SilentlyContinue)) { throw "pwsh was not found in PATH." }
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) { throw "dotnet was not found in PATH." }

if (-not $SkipTests) {
    Invoke-Step "Release readiness tests" {
        & dotnet test $testProject -c Release --filter "FullyQualifiedName~GooglePlayReleaseReadinessTests|FullyQualifiedName~ProductionReadinessTests" --verbosity minimal
        if ($LASTEXITCODE -ne 0) { throw "Release readiness tests failed." }
    }
}
else {
    Add-Step "Release readiness tests" "SKIPPED" "-SkipTests used"
}

if (-not $SkipAndroidBuild) {
    Invoke-Step "Android Release build" {
        & dotnet build $mobileProject -f net9.0-android -c Release -p:EnableLocalAiRelease=false --verbosity minimal
        if ($LASTEXITCODE -ne 0) { throw "Android Release build failed." }
    }
}
else {
    Add-Step "Android Release build" "SKIPPED" "-SkipAndroidBuild used"
}

Invoke-Step "Generate Google Play graphics" {
    & pwsh (Join-Path $PSScriptRoot "generate-play-graphics.ps1") -RepoRoot $RepoRoot
    if ($LASTEXITCODE -ne 0) { throw "Graphics generation failed." }
}

Invoke-Step "Export release metadata" {
    & pwsh (Join-Path $PSScriptRoot "export-play-metadata.ps1") -RepoRoot $RepoRoot
    if ($LASTEXITCODE -ne 0) { throw "Metadata export failed." }
}

Invoke-Step "Validate public site source" {
    foreach ($relative in @(
        "site\index.html",
        "site\privacy\index.html",
        "site\support\index.html",
        "site\terms\index.html"
    )) {
        $path = Join-Path $RepoRoot $relative
        if (-not (Test-Path $path)) { throw "Public site file missing: $relative" }
    }
}

$screenshotDirectory = Join-Path $resultsRoot "screenshots\final"
$requiredScreenshots = @(
    "01-catalog.png",
    "02-categories.png",
    "03-quiz.png",
    "04-result.png",
    "05-history-favorites.png",
    "06-privacy-settings.png"
)

$missingScreenshots = $requiredScreenshots | Where-Object { -not (Test-Path (Join-Path $screenshotDirectory $_)) }
if ($missingScreenshots.Count -eq 0) {
    Add-Step "Final screenshots" "PASS" "6/6"
}
elseif ($RequireScreenshots) {
    Add-Step "Final screenshots" "FAIL" ("Missing: " + ($missingScreenshots -join ", "))
    Write-Summary
    throw "Required Google Play screenshots are incomplete."
}
else {
    Add-Step "Final screenshots" "PENDING_OWNER_ACTION" ("Missing: " + ($missingScreenshots -join ", "))
}

$signingArgumentsProvided = -not [string]::IsNullOrWhiteSpace($KeystorePath) `
    -or -not [string]::IsNullOrWhiteSpace($KeyPasswordFile) `
    -or -not [string]::IsNullOrWhiteSpace($StorePasswordFile)

if ($signingArgumentsProvided) {
    if ([string]::IsNullOrWhiteSpace($KeystorePath) -or [string]::IsNullOrWhiteSpace($KeyPasswordFile) -or [string]::IsNullOrWhiteSpace($StorePasswordFile)) {
        throw "KeystorePath, KeyPasswordFile and StorePasswordFile must be supplied together."
    }

    Invoke-Step "Signed Google Play AAB" {
        $arguments = @(
            "-NoProfile", "-File", (Join-Path $PSScriptRoot "build-play-aab.ps1"),
            "-RepoRoot", $RepoRoot,
            "-KeystorePath", $KeystorePath,
            "-KeyAlias", $KeyAlias,
            "-KeyPasswordFile", $KeyPasswordFile,
            "-StorePasswordFile", $StorePasswordFile,
            "-SkipTests"
        )
        & pwsh @arguments
        if ($LASTEXITCODE -ne 0) { throw "Signed AAB build failed." }
    }
}
else {
    Add-Step "Signed Google Play AAB" "PENDING_OWNER_ACTION" "Signing secrets were not supplied"
}

Write-Summary
Write-Host ""
Write-Host "Google Play package preparation finished."
Write-Host "Summary: $summaryPath"
