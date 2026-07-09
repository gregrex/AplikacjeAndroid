# Narzędzia jakości AppFactory

Ten katalog zawiera skrypty pomocnicze do utrzymania jakości paczek projektów.

## Skrypty

### `sync-runtime-packs.ps1`

Kopiuje dane źródłowe z:

```text
projects/<projectId>/data
projects/<projectId>/theme.json
```

do runtime aplikacji:

```text
src/AppFactory.Mobile/wwwroot/projects/<projectId>
```

Uruchomienie testowe bez zapisu:

```powershell
pwsh ./tools/quality/sync-runtime-packs.ps1 -WhatIfOnly
```

Uruchomienie właściwe:

```powershell
pwsh ./tools/quality/sync-runtime-packs.ps1
```

### `write-project-quality-report.ps1`

Generuje markdownowy raport jakości do:

```text
docs/quality/PROJECTS_REPORT.md
```

Uruchomienie:

```powershell
pwsh ./tools/quality/write-project-quality-report.ps1
```

Raport pokazuje:

- czy projekt jest w `ProjectCatalogService`,
- czy ma folder źródłowy,
- czy ma komplet danych źródłowych,
- czy ma komplet runtime,
- czy ma theme,
- czy ma marketing,
- czy ma manual tests.

### `run-quality-checks.ps1`

Uruchamia testy jakości projektu.

```powershell
pwsh ./tools/quality/run-quality-checks.ps1
```

Z synchronizacją runtime przed testami:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst
```

Z wygenerowaniem raportu przed testami:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -WriteReport
```

Pełny przebieg: synchronizacja runtime, raport i testy:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport
```

## Co sprawdzają testy globalne

Test `AllProjectsQualityTests` sprawdza:

- czy każdy projekt z katalogu ma źródłowy katalog `projects/<id>`,
- czy każdy projekt ma komplet plików `data`,
- czy każdy projekt ma runtime mirror w `wwwroot`,
- czy każdy projekt ma `theme.json`,
- czy każdy projekt ma listing marketingowy PL,
- czy każdy projekt ma manual tests,
- czy dane przechodzą `DataPackValidationService`,
- czy `app.json` ma poprawne podstawowe pola,
- czy `theme.json` ma poprawne podstawowe pola,
- czy PL/EN/UK mają te same `resultId`,
- czy runtime ma tę samą strukturę ID co źródło.

## Zalecany lokalny proces

1. Uruchom pełną ścieżkę jakości:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport
```

2. Jeśli testy pokażą błąd, napraw pierwszy projekt z listy błędów.

3. Powtórz testy.

## Uwaga

Skrypty nie publikują aplikacji i nie budują paczek Android. To tylko etap jakości danych oraz testów.
