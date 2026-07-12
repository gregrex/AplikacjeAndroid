param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [string]$InputFile = (Join-Path $RepoRoot "marketing\google-play\listings.json"),
    [string]$LocalePlanFile = (Join-Path $RepoRoot "marketing\google-play\release-locales.json"),
    [string]$OutputDirectory = (Join-Path $RepoRoot "artifacts\google-play\metadata\android"),
    [switch]$IncludePlannedLocales
)

$ErrorActionPreference = "Stop"

foreach ($required in @($InputFile, $LocalePlanFile)) {
    if (-not (Test-Path $required)) {
        throw "Required Google Play metadata file not found: $required"
    }
}

$data = Get-Content $InputFile -Raw -Encoding UTF8 | ConvertFrom-Json
$localePlan = Get-Content $LocalePlanFile -Raw -Encoding UTF8 | ConvertFrom-Json
$errors = New-Object System.Collections.Generic.List[string]

if ($data.locales.PSObject.Properties.Count -ne 25) {
    $errors.Add("Expected 25 prepared locales: 24 official EU languages plus Ukrainian. Found $($data.locales.PSObject.Properties.Count).")
}

$preparedLocaleNames = $data.locales.PSObject.Properties.Name
$plannedNames = @($localePlan.releaseLocales) + @($localePlan.plannedLocales)
if ($plannedNames.Count -ne 25 -or ($plannedNames | Select-Object -Unique).Count -ne 25) {
    $errors.Add("Locale release plan must contain 25 unique locales.")
}

foreach ($locale in $plannedNames) {
    if ($locale -notin $preparedLocaleNames) {
        $errors.Add("Locale plan references missing listing: $locale")
    }
}

foreach ($localeProperty in $data.locales.PSObject.Properties) {
    $locale = $localeProperty.Name
    $listing = $localeProperty.Value

    if ([string]::IsNullOrWhiteSpace($listing.title) -or $listing.title.Length -gt 30) {
        $errors.Add("$locale title must contain 1-30 characters; found $($listing.title.Length).")
    }
    if ([string]::IsNullOrWhiteSpace($listing.shortDescription) -or $listing.shortDescription.Length -gt 80) {
        $errors.Add("$locale short description must contain 1-80 characters; found $($listing.shortDescription.Length).")
    }
    if ([string]::IsNullOrWhiteSpace($listing.fullDescription) -or $listing.fullDescription.Length -gt 4000) {
        $errors.Add("$locale full description must contain 1-4000 characters; found $($listing.fullDescription.Length).")
    }
}

if ($errors.Count -gt 0) {
    throw "Google Play listing validation failed:`n$($errors -join [Environment]::NewLine)"
}

$localesToExport = if ($IncludePlannedLocales) {
    $preparedLocaleNames
}
else {
    @($localePlan.releaseLocales)
}

if (Test-Path $OutputDirectory) {
    Remove-Item $OutputDirectory -Recurse -Force
}
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

foreach ($locale in $localesToExport) {
    $listing = $data.locales.$locale
    $localeDirectory = Join-Path $OutputDirectory $locale
    New-Item -ItemType Directory -Force -Path $localeDirectory | Out-Null

    Set-Content -Path (Join-Path $localeDirectory "title.txt") -Value $listing.title -Encoding UTF8 -NoNewline
    Set-Content -Path (Join-Path $localeDirectory "short_description.txt") -Value $listing.shortDescription -Encoding UTF8 -NoNewline
    Set-Content -Path (Join-Path $localeDirectory "full_description.txt") -Value $listing.fullDescription -Encoding UTF8 -NoNewline
}

$metadata = [ordered]@{
    packageName = $data.packageName
    defaultLocale = $data.defaultLocale
    category = $data.category
    contactEmail = $data.contactEmail
    privacyPolicyUrl = $data.privacyPolicyUrl
    supportUrl = $data.supportUrl
    preparedLocaleCount = $data.locales.PSObject.Properties.Count
    exportedLocaleCount = $localesToExport.Count
    exportedLocales = @($localesToExport)
    plannedLocalesIncluded = [bool]$IncludePlannedLocales
    generatedAtUtc = (Get-Date).ToUniversalTime().ToString("o")
}

$metadata | ConvertTo-Json -Depth 4 | Set-Content -Path (Join-Path (Split-Path $OutputDirectory -Parent) "release-metadata.json") -Encoding UTF8
Write-Host "Google Play metadata exported for $($metadata.exportedLocaleCount) locales to $OutputDirectory"
if (-not $IncludePlannedLocales) {
    Write-Host "Only fully supported in-app languages were exported: $($localesToExport -join ', ')"
    Write-Host "Use -IncludePlannedLocales only after completing and testing all matching in-app translations."
}
