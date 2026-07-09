param(
    [string]$RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path,
    [switch]$WhatIfOnly
)

$ErrorActionPreference = "Stop"

$projectsRoot = Join-Path $RepoRoot "projects"
$runtimeRoot = Join-Path $RepoRoot "src\AppFactory.Mobile\wwwroot\projects"

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

if (-not (Test-Path $runtimeRoot)) {
    if ($WhatIfOnly) {
        Write-Host "[WHATIF] Create runtime root: $runtimeRoot"
    } else {
        New-Item -ItemType Directory -Force -Path $runtimeRoot | Out-Null
    }
}

$projects = Get-ChildItem -Path $projectsRoot -Directory | Sort-Object Name
$errors = New-Object System.Collections.Generic.List[string]
$copied = 0
$createdDirectories = 0

foreach ($project in $projects) {
    $projectId = $project.Name
    $sourceDataDir = Join-Path $project.FullName "data"
    $sourceTheme = Join-Path $project.FullName "theme.json"
    $runtimeDir = Join-Path $runtimeRoot $projectId

    if (-not (Test-Path $sourceDataDir)) {
        $errors.Add("$projectId: missing data directory")
        continue
    }

    if (-not (Test-Path $runtimeDir)) {
        if ($WhatIfOnly) {
            Write-Host "[WHATIF] Create runtime directory: $runtimeDir"
        } else {
            New-Item -ItemType Directory -Force -Path $runtimeDir | Out-Null
        }
        $createdDirectories++
    }

    foreach ($file in $requiredDataFiles) {
        $sourceFile = Join-Path $sourceDataDir $file
        $targetFile = Join-Path $runtimeDir $file

        if (-not (Test-Path $sourceFile)) {
            $errors.Add("$projectId: missing source data file $file")
            continue
        }

        if ($WhatIfOnly) {
            Write-Host "[WHATIF] Copy $sourceFile -> $targetFile"
        } else {
            Copy-Item -Path $sourceFile -Destination $targetFile -Force
        }
        $copied++
    }

    if (-not (Test-Path $sourceTheme)) {
        $errors.Add("$projectId: missing theme.json")
        continue
    }

    $targetTheme = Join-Path $runtimeDir "theme.json"
    if ($WhatIfOnly) {
        Write-Host "[WHATIF] Copy $sourceTheme -> $targetTheme"
    } else {
        Copy-Item -Path $sourceTheme -Destination $targetTheme -Force
    }
    $copied++
}

Write-Host "Runtime sync summary"
Write-Host "Projects scanned: $($projects.Count)"
Write-Host "Runtime directories created: $createdDirectories"
Write-Host "Files copied: $copied"

if ($errors.Count -gt 0) {
    Write-Host "Errors:"
    foreach ($errorMessage in $errors) {
        Write-Host " - $errorMessage"
    }
    exit 1
}

Write-Host "Runtime sync completed without errors."
