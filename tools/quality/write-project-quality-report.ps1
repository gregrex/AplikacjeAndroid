param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [string]$OutputPath = (Join-Path (Join-Path (Join-Path (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path "docs") "quality")
)

$ErrorActionPreference = "Stop"

$projectsRoot = Join-Path $RepoRoot "projects"
$runtimeRoot = Join-Path $RepoRoot "src\AppFactory.Mobile\wwwroot\projects"
$catalogPath = Join-Path $RepoRoot "src\AppFactory.Mobile\Services\ProjectCatalogService.cs"
$reportPath = Join-Path $OutputPath "PROJECTS_REPORT.md"

$requiredDataFiles = @(
    "app.json",
    "categories.json",
    "questions.json",
    "rules.json",
    "results.pl.json",
    "results.en.json",
    "results.uk.json"
)

if (-not (Test-Path $projectsRoot)) {
    throw "Projects directory not found: $projectsRoot"
}

if (-not (Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Force -Path $OutputPath | Out-Null
}

$catalogIds = New-Object System.Collections.Generic.List[string]
if (Test-Path $catalogPath) {
    $catalogContent = Get-Content -Raw -Path $catalogPath
    $matches = [regex]::Matches($catalogContent, 'new\("([^"]+)"')
    foreach ($match in $matches) {
        $catalogIds.Add($match.Groups[1].Value)
    }
}

$projectIds = Get-ChildItem -Path $projectsRoot -Directory | Select-Object -ExpandProperty Name | Sort-Object
$allIds = @($catalogIds + $projectIds) | Sort-Object -Unique

$lines = New-Object System.Collections.Generic.List[string]
$lines.Add("# Raport jakości projektów")
$lines.Add("")
$lines.Add("Wygenerowano lokalnie skryptem `tools/quality/write-project-quality-report.ps1`.")
$lines.Add("")
$lines.Add("## Podsumowanie")
$lines.Add("")
$lines.Add("- Projekty w katalogu: $($catalogIds.Count)")
$lines.Add("- Foldery w `projects`: $($projectIds.Count)")
$lines.Add("- Unikalne ID do kontroli: $($allIds.Count)")
$lines.Add("- Wymagane scenariusze produkcyjne: 5 na projekt")
$lines.Add("")
$lines.Add("## Tabela kontroli")
$lines.Add("")
$lines.Add("| Projekt | W katalogu | Folder source | Data source | Runtime | Theme | Marketing | Manual tests | 5 scenariuszy | Uwagi |")
$lines.Add("|---|---:|---:|---:|---:|---:|---:|---:|---:|---|")

foreach ($projectId in $allIds) {
    $notes = New-Object System.Collections.Generic.List[string]
    $inCatalog = $catalogIds -contains $projectId
    $sourceDir = Join-Path $projectsRoot $projectId
    $dataDir = Join-Path $sourceDir "data"
    $runtimeDir = Join-Path $runtimeRoot $projectId

    $hasSourceDir = Test-Path $sourceDir
    $hasDataDir = Test-Path $dataDir
    $hasRuntimeDir = Test-Path $runtimeDir
    $hasTheme = (Test-Path (Join-Path $sourceDir "theme.json")) -and (Test-Path (Join-Path $runtimeDir "theme.json"))
    $hasMarketing = Test-Path (Join-Path (Join-Path $sourceDir "marketing") "store-listing.pl.md")
    $hasManualTests = Test-Path (Join-Path (Join-Path $sourceDir "tests") "manual-tests.md")
    $scenarioPath = Join-Path (Join-Path $sourceDir "tests") "production-scenarios.md"
    $hasProductionScenarios = $false

    if (Test-Path $scenarioPath) {
        $scenarioContent = Get-Content -Raw -Path $scenarioPath
        $scenarioCount = [regex]::Matches($scenarioContent, '(?m)^## SCN-\d{2}\s+—\s+.+$').Count
        $hasProductionScenarios = $scenarioCount -eq 5
        if (-not $hasProductionScenarios) {
            $notes.Add("expected 5 production scenarios, found $scenarioCount")
        }
    }
    else {
        $notes.Add("missing production scenarios")
    }

    $dataOk = $true
    $runtimeOk = $true

    foreach ($file in $requiredDataFiles) {
        if (-not (Test-Path (Join-Path $dataDir $file))) {
            $dataOk = $false
            $notes.Add("missing source $file")
        }

        if (-not (Test-Path (Join-Path $runtimeDir $file))) {
            $runtimeOk = $false
            $notes.Add("missing runtime $file")
        }
    }

    if (-not $inCatalog) { $notes.Add("not in ProjectCatalogService") }
    if (-not $hasSourceDir) { $notes.Add("missing source folder") }
    if (-not $hasTheme) { $notes.Add("missing source/runtime theme") }
    if (-not $hasMarketing) { $notes.Add("missing PL listing") }
    if (-not $hasManualTests) { $notes.Add("missing manual tests") }

    if ($notes.Count -eq 0) {
        $notes.Add("OK")
    }

    $lines.Add("| `$projectId` | $(ToMark $inCatalog) | $(ToMark $hasSourceDir) | $(ToMark ($hasDataDir -and $dataOk)) | $(ToMark ($hasRuntimeDir -and $runtimeOk)) | $(ToMark $hasTheme) | $(ToMark $hasMarketing) | $(ToMark $hasManualTests) | $(ToMark $hasProductionScenarios) | $($notes -join '; ') |")
}

$lines.Add("")
$lines.Add("## Następny krok")
$lines.Add("")
$lines.Add("Po wygenerowaniu raportu uruchom:")
$lines.Add("")
$lines.Add("```powershell")
$lines.Add("pwsh ./tools/quality/run-quality-checks.ps1")
$lines.Add("```")
$lines.Add("")

Set-Content -Path $reportPath -Value $lines -Encoding UTF8
Write-Host "Report written to: $reportPath"

function ToMark([bool]$value) {
    if ($value) { return "✅" }
    return "❌"
}
