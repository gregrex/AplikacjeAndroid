param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [Parameter(Mandatory = $true)][string]$KeystorePath,
    [string]$KeyAlias = "appfactory-upload",
    [Parameter(Mandatory = $true)][string]$KeyPasswordFile,
    [Parameter(Mandatory = $true)][string]$StorePasswordFile,
    [switch]$SkipTests,
    [switch]$EnableLocalAiRelease
)

$ErrorActionPreference = "Stop"

$project = Join-Path $RepoRoot "src\AppFactory.Mobile\AppFactory.Mobile.csproj"
$testProject = Join-Path $RepoRoot "tests\AppFactory.Mobile.Tests\AppFactory.Mobile.Tests.csproj"
$outputDirectory = Join-Path $RepoRoot "artifacts\google-play\release"

foreach ($requiredPath in @($project, $KeystorePath, $KeyPasswordFile, $StorePasswordFile)) {
    if (-not (Test-Path $requiredPath)) {
        throw "Required file not found: $requiredPath"
    }
}

$repoFullPath = [System.IO.Path]::GetFullPath($RepoRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar) + [System.IO.Path]::DirectorySeparatorChar
foreach ($secretPath in @($KeystorePath, $KeyPasswordFile, $StorePasswordFile)) {
    $fullSecretPath = [System.IO.Path]::GetFullPath($secretPath)
    if ($fullSecretPath.StartsWith($repoFullPath, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Signing secret must be stored outside the repository: $fullSecretPath"
    }
}

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw "dotnet was not found in PATH."
}

New-Item -ItemType Directory -Force -Path $outputDirectory | Out-Null

if (-not $SkipTests) {
    Write-Host "Running release tests..."
    & dotnet test $testProject -c Release --verbosity minimal
    if ($LASTEXITCODE -ne 0) {
        throw "Tests failed. AAB was not built."
    }
}

$localAiValue = if ($EnableLocalAiRelease) { "true" } else { "false" }
if ($EnableLocalAiRelease) {
    Write-Warning "Local AI is being enabled in Release. Confirm real model URLs, SHA256, labels, privacy policy and Data Safety before upload."
}

Write-Host "Publishing signed AppFactory AAB..."
& dotnet publish $project `
    -f net9.0-android `
    -c Release `
    --verbosity minimal `
    -p:AndroidPackageFormats=aab `
    -p:AndroidKeyStore=true `
    -p:AndroidSigningKeyStore="$KeystorePath" `
    -p:AndroidSigningKeyAlias="$KeyAlias" `
    -p:AndroidSigningKeyPass="file:$KeyPasswordFile" `
    -p:AndroidSigningStorePass="file:$StorePasswordFile" `
    -p:EnableLocalAiRelease=$localAiValue

if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed."
}

$publishDirectory = Join-Path $RepoRoot "src\AppFactory.Mobile\bin\Release\net9.0-android\publish"
$aab = Get-ChildItem $publishDirectory -Filter "*-signed.aab" -File -ErrorAction SilentlyContinue |
    Sort-Object LastWriteTimeUtc -Descending |
    Select-Object -First 1

if ($null -eq $aab) {
    $aab = Get-ChildItem $publishDirectory -Filter "*.aab" -File -ErrorAction SilentlyContinue |
        Sort-Object LastWriteTimeUtc -Descending |
        Select-Object -First 1
}

if ($null -eq $aab) {
    throw "No AAB was found in $publishDirectory"
}

$destination = Join-Path $outputDirectory "appfactory-pomocniki-1.0.0-build1.aab"
Copy-Item $aab.FullName $destination -Force
$hash = Get-FileHash $destination -Algorithm SHA256
$hash.Hash.ToLowerInvariant() | Set-Content "$destination.sha256" -Encoding ASCII -NoNewline

$manifest = [ordered]@{
    applicationId = "pl.gbcom.appfactory"
    applicationTitle = "AppFactory Pomocniki"
    displayVersion = "1.0.0"
    versionCode = 1
    targetApi = 35
    localAiRelease = [bool]$EnableLocalAiRelease
    adsEnabled = $false
    builtAtUtc = (Get-Date).ToUniversalTime().ToString("o")
    file = (Split-Path $destination -Leaf)
    sha256 = $hash.Hash.ToLowerInvariant()
}
$manifest | ConvertTo-Json | Set-Content (Join-Path $outputDirectory "release-manifest.json") -Encoding UTF8

Write-Host ""
Write-Host "Signed AAB ready: $destination"
Write-Host "SHA256: $($hash.Hash.ToLowerInvariant())"
Write-Host "Upload this file to Google Play Internal testing before any production rollout."
