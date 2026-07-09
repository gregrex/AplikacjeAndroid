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

### `run-quality-checks.ps1`

Uruchamia testy jakości projektu.

```powershell
pwsh ./tools/quality/run-quality-checks.ps1
```

Z synchronizacją runtime przed testami:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst
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
- czy PL/EN/UK mają te same `resultId`,
- czy runtime ma tę samą strukturę ID co źródło.

## Zalecany lokalny proces

1. Uruchom synchronizację runtime:

```powershell
pwsh ./tools/quality/sync-runtime-packs.ps1
```

2. Uruchom testy:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1
```

3. Jeśli testy pokażą błąd, napraw pierwszy projekt z listy błędów.

4. Powtórz testy.

## Uwaga

Skrypty nie publikują aplikacji i nie budują paczek Android. To tylko etap jakości danych oraz testów.
