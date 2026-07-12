param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [string]$ArtifactsRoot = (Join-Path $RepoRoot "artifacts\google-play"),
    [switch]$RequireSignedAab,
    [switch]$RequireScreenshots
)

$ErrorActionPreference = "Stop"
$errors = New-Object System.Collections.Generic.List[string]
$warnings = New-Object System.Collections.Generic.List[string]

function Require-File([string]$Path, [string]$Label) {
    if (-not (Test-Path $Path)) {
        $errors.Add("Missing $Label: $Path")
        return $false
    }
    return $true
}

function Check-Image {
    param([string]$Path, [int]$ExpectedWidth, [int]$ExpectedHeight, [long]$MaxBytes = 0)

    if (-not (Require-File $Path "image")) { return }

    if ($IsWindows) {
        Add-Type -AssemblyName System.Drawing.Common
        $image = [System.Drawing.Image]::FromFile($Path)
        try {
            if ($image.Width -ne $ExpectedWidth -or $image.Height -ne $ExpectedHeight) {
                $errors.Add("Invalid image dimensions for $Path: $($image.Width)x$($image.Height), expected ${ExpectedWidth}x${ExpectedHeight}.")
            }
        }
        finally {
            $image.Dispose()
        }
    }
    else {
        $warnings.Add("Image dimensions were not inspected because System.Drawing validation is Windows-only: $Path")
    }

    if ($MaxBytes -gt 0) {
        $length = (Get-Item $Path).Length
        if ($length -gt $MaxBytes) {
            $errors.Add("Image exceeds size limit: $Path ($length bytes > $MaxBytes bytes).")
        }
    }
}

$generatedRoot = Join-Path $ArtifactsRoot "generated"
$metadataRoot = Join-Path $ArtifactsRoot "metadata\android"
$releaseMetadata = Join-Path $ArtifactsRoot "metadata\release-metadata.json"
$releaseRoot = Join-Path $ArtifactsRoot "release"
$screenshotRoot = Join-Path $ArtifactsRoot "screenshots\final"

Check-Image (Join-Path $generatedRoot "app-icon-512.png") 512 512 1MB
Check-Image (Join-Path $generatedRoot "feature-graphic-1024x500.png") 1024 500
foreach ($locale in @("pl-PL", "en-US", "uk-UA")) {
    Check-Image (Join-Path $generatedRoot "feature-graphic-$locale-1024x500.png") 1024 500
}

foreach ($locale in @("pl-PL", "en-US", "uk-UA")) {
    $localeRoot = Join-Path $metadataRoot $locale
    foreach ($name in @("title.txt", "short_description.txt", "full_description.txt", "changelogs\1.txt")) {
        $path = Join-Path $localeRoot $name
        if (Require-File $path "$locale metadata") {
            $text = Get-Content $path -Raw -Encoding UTF8
            if ([string]::IsNullOrWhiteSpace($text)) {
                $errors.Add("Metadata file is empty: $path")
            }
        }
    }
}

if (Require-File $releaseMetadata "release metadata manifest") {
    $metadata = Get-Content $releaseMetadata -Raw -Encoding UTF8 | ConvertFrom-Json
    if ($metadata.packageName -ne "pl.gbcom.appfactory") { $errors.Add("Unexpected packageName in release metadata: $($metadata.packageName)") }
    if ($metadata.versionCode -ne 1) { $errors.Add("Unexpected versionCode in release metadata: $($metadata.versionCode)") }
    if ($metadata.exportedLocaleCount -ne 3) { $errors.Add("Release should export exactly three fully supported locales; found $($metadata.exportedLocaleCount).") }
}

$requiredScreenshots = @(
    "01-catalog.png",
    "02-categories.png",
    "03-quiz.png",
    "04-result.png",
    "05-history-favorites.png",
    "06-privacy-settings.png"
)

foreach ($name in $requiredScreenshots) {
    $path = Join-Path $screenshotRoot $name
    if (Test-Path $path) {
        if ($IsWindows) {
            Add-Type -AssemblyName System.Drawing.Common
            $image = [System.Drawing.Image]::FromFile($path)
            try {
                if ($image.Height -le $image.Width) {
                    $errors.Add("Phone screenshot is not portrait: $path ($($image.Width)x$($image.Height)).")
                }
                if ($image.Width -lt 320 -or $image.Height -lt 320) {
                    $errors.Add("Screenshot is too small: $path ($($image.Width)x$($image.Height)).")
                }
            }
            finally {
                $image.Dispose()
            }
        }
    }
    elseif ($RequireScreenshots) {
        $errors.Add("Missing final screenshot: $path")
    }
    else {
        $warnings.Add("Pending final screenshot: $path")
    }
}

$aab = Get-ChildItem $releaseRoot -Filter "*.aab" -File -ErrorAction SilentlyContinue | Select-Object -First 1
if ($null -eq $aab) {
    if ($RequireSignedAab) {
        $errors.Add("Signed AAB is required but was not found in $releaseRoot")
    }
    else {
        $warnings.Add("Signed AAB is pending owner signing action.")
    }
}
else {
    $hashPath = "$($aab.FullName).sha256"
    $manifestPath = Join-Path $releaseRoot "release-manifest.json"
    Require-File $hashPath "AAB SHA256" | Out-Null
    if (Require-File $manifestPath "AAB release manifest") {
        $manifest = Get-Content $manifestPath -Raw -Encoding UTF8 | ConvertFrom-Json
        if ($manifest.applicationId -ne "pl.gbcom.appfactory") { $errors.Add("AAB manifest has unexpected applicationId.") }
        if ($manifest.displayVersion -ne "1.0.0") { $errors.Add("AAB manifest has unexpected displayVersion.") }
        if ($manifest.versionCode -ne 1) { $errors.Add("AAB manifest has unexpected versionCode.") }
        if ($manifest.targetApi -lt 35) { $errors.Add("AAB manifest target API is below 35.") }
        if ($manifest.localAiRelease -ne $false) { $errors.Add("Local AI must remain disabled for version 1.0.") }
        if ($manifest.adsEnabled -ne $false) { $errors.Add("Ads must remain disabled for version 1.0.") }
    }
}

Write-Host ""
Write-Host "Google Play package verification"
Write-Host "Errors: $($errors.Count)"
Write-Host "Warnings: $($warnings.Count)"
foreach ($warning in $warnings) { Write-Warning $warning }

if ($errors.Count -gt 0) {
    foreach ($errorMessage in $errors) { Write-Error $errorMessage }
    throw "Google Play package verification failed with $($errors.Count) error(s)."
}

Write-Host "Verification passed."
if ($warnings.Count -gt 0) {
    Write-Host "The package is technically consistent but still has pending owner artifacts."
}
