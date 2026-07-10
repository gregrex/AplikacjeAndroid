# Status jakości projektów AppFactory

## Status produkcyjny

Aktualny status repo: **production-ready candidate**.

Repo ma komplet danych, testów strukturalnych i scenariuszy akceptacyjnych. Finalny status `production-ready` wymaga zielonego CI oraz wykonania scenariuszy na Androidzie.

## Zakres katalogu

Katalog obejmuje 20 projektów.

## Scenariusze produkcyjne

Dla każdego projektu dodano:

```text
projects/<projectId>/tests/production-scenarios.md
```

Każdy plik ma dokładnie pięć scenariuszy `SCN-01`–`SCN-05` zawierających:

- cel,
- numerowane kroki,
- oczekiwany wynik,
- opis pokrycia.

Łączne pokrycie:

- **20 projektów**,
- **5 scenariuszy na projekt**,
- **100 scenariuszy produkcyjnych**.

Raport:

```text
docs/quality/PROJECT_SCENARIO_COVERAGE.md
```

Automatyczny gate:

```text
tests/AppFactory.Mobile.Tests/ProjectProductionScenariosTests.cs
```

sprawdza:

- istnienie pliku dla każdego projektu,
- dokładnie pięć scenariuszy,
- identyfikatory `SCN-01`–`SCN-05`,
- sekcje `Cel`, `Kroki`, `Oczekiwany wynik`, `Pokrycie`,
- minimum dwa kroki na scenariusz,
- scenariusz `local-vision-v1` + ONNX dla projektów obrazowych,
- scenariusz `local-audio-v1` + ONNX dla projektów dźwiękowych.

Generator raportu `write-project-quality-report.ps1` pokazuje osobną kolumnę pokrycia pięciu scenariuszy.

## Testy globalne i produkcyjne

Repo zawiera między innymi:

- `AllProjectsQualityTests.cs`,
- `RuleReasonsQualityTests.cs`,
- `ProductionReadinessTests.cs`,
- `ProjectProductionScenariosTests.cs`,
- `RuleEngineServiceTests.cs`,
- `MatchInfoParserTests.cs`,
- `ResultNavigationStateServiceTests.cs`,
- `BuildProfileServiceTests.cs`,
- `ImageAnalysisServiceTests.cs`,
- `AudioAnalysisServiceTests.cs`,
- `LocalAiModelStoreTests.cs`,
- `LocalAiInputTensorFactoryTests.cs`.

## Local AI on device

Wybrany runtime:

```text
Microsoft.ML.OnnxRuntime
```

Pakiet jest w projektach Core i Mobile.

Warstwa lokalnego AI obejmuje:

- `LocalAiModelCatalogService`,
- `LocalAiModelStore`,
- `OnnxModelRunner`,
- `LocalAiInputTensorFactory`,
- `OnDeviceImageAnalysisProvider`,
- `LocalVisionInferenceEngine`,
- `OnDeviceAudioAnalysisProvider`,
- `LocalAudioInferenceEngine`.

Aktualny kontrakt wejścia:

```text
input: float32[1,1,1,256]
```

Modele `local-vision-v1` i `local-audio-v1` wymagają skonfigurowania adresu pobierania, SHA256 i docelowych etykiet klas.

## Pozostałe wykonane elementy

- komplet source i runtime dla 20 projektów,
- `reason` dla wszystkich reguł,
- parytet wyników PL/EN/UK,
- build profiles dla katalogu i osobnych aplikacji,
- CI `Quality Checks`,
- wyszukiwarka i filtrowanie katalogu,
- ulepszony quiz,
- stan wyniku poza query string,
- alternatywne rekomendacje,
- historia i ulubione,
- UI pobierania lokalnych modeli.

## Weryfikacja wymagana przed publikacją

1. Uruchomić:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport
```

2. Wykonać 100 scenariuszy na emulatorze lub urządzeniu Android.
3. Zapisać PASS/FAIL dla każdego scenariusza.
4. Naprawić wszystkie błędy krytyczne i wysokie.
5. Skonfigurować docelowe modele ONNX lub wyłączyć funkcje Local AI w buildzie publikacyjnym do czasu gotowości modeli.

## Uwagi

Nie uruchamiałem lokalnie `dotnet test`, ponieważ zmiany wykonuję przez GitHub connector. Dostępne wywołanie workflow dla sprawdzanego commita nie zostało zwrócone przez connector, więc nie oznaczam testów jako wykonanych.
