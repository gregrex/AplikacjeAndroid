param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [string]$OutputPath = (Join-Path $RepoRoot "docs\quality\BUILD_PROFILES.md")
)

$ErrorActionPreference = "Stop"

$catalogPath = Join-Path $RepoRoot "src\AppFactory.Mobile\Services\ProjectCatalogService.cs"
if (-not (Test-Path $catalogPath)) {
    throw "Project catalog not found: $catalogPath"
}

$content = Get-Content $catalogPath -Raw
$matches = [regex]::Matches($content, 'new\("(?<id>[^"]+)",\s*"(?<name>[^"]+)",\s*"(?<description>[^"]+)"\)')
if ($matches.Count -eq 0) {
    throw "No projects found in catalog."
}

$lines = New-Object System.Collections.Generic.List[string]
$lines.Add("# Build profiles AppFactory")
$lines.Add("")
$lines.Add("Ten raport opisuje katalogowy build oraz osobne profile buildów dla mikroaplikacji.")
$lines.Add("")
$lines.Add("## Catalog build")
$lines.Add("")
$lines.Add("| ProjectId | ApplicationTitle | ApplicationId |")
$lines.Add("| --- | --- | --- |")
$lines.Add("| catalog | AppFactory | pl.gbcom.appfactory |")
$lines.Add("")
$lines.Add("## Project builds")
$lines.Add("")
$lines.Add("| ProjectId | ApplicationTitle | ApplicationId |")
$lines.Add("| --- | --- | --- |")

foreach ($match in $matches) {
    $id = $match.Groups["id"].Value
    $name = $match.Groups["name"].Value
    $safeId = $id -replace '-', ''
    $applicationId = "pl.gbcom.appfactory.$safeId"
    $lines.Add("| `$id` | $name | `$applicationId` |")
}

$lines.Add("")
$lines.Add("## Użycie")
$lines.Add("")
$lines.Add("Docelowy build osobnej aplikacji powinien przekazywać `ProjectId`, `ApplicationTitle` i `ApplicationId` z powyższej tabeli do pipeline publikacyjnego.")
$lines.Add("")
$lines.Add("Na tym etapie raport jest źródłem prawdy dla wariantów buildów i punktem startowym do konfiguracji flavorów / pipeline per aplikacja.")

$directory = Split-Path $OutputPath -Parent
if (-not (Test-Path $directory)) {
    New-Item -ItemType Directory -Path $directory | Out-Null
}

Set-Content -Path $OutputPath -Value $lines -Encoding UTF8
Write-Host "Build profiles report written to $OutputPath"
