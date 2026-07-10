# Status jakości projektów AppFactory

## Data kontroli

Etap jakościowy rozpoczęty po domknięciu 20 projektów MVP.

## Status produkcyjny

Aktualny status repo: **production-ready candidate**.

Nie oznaczam jeszcze jako final `production-ready`, dopóki nie przejdzie lokalny `dotnet test`, skrypt jakości albo workflow GitHub Actions `Quality Checks`.

## Zakres katalogu

Katalog aplikacji obejmuje 20 projektów.

## Wykonane w etapie jakościowym

### Dokumentacja

Dodano dokumenty jakości, produkcji, build profiles, image analysis oraz local on-device AI:

- `docs/quality/IMAGE_ANALYSIS_V1.md`,
- `docs/quality/LOCAL_AI_ON_DEVICE.md`.

### CI

Dodano `.github/workflows/quality-checks.yml`.

### Testy globalne i produkcyjne

Dodano testy danych, reguł, produkcji, build profiles, stanu wyniku, analizy obrazu, analizy dźwięku i pobierania modeli:

- `tests/AppFactory.Mobile.Tests/ImageAnalysisServiceTests.cs`,
- `tests/AppFactory.Mobile.Tests/AudioAnalysisServiceTests.cs`,
- `tests/AppFactory.Mobile.Tests/LocalAiModelStoreTests.cs`.

### Local AI on device

Dodano lokalną warstwę AI dla telefonu:

- `LocalAiModelProfile`,
- `LocalAiModelStatus`,
- `LocalAiModelDownloadResult`,
- `LocalAiModelCatalogService`,
- `LocalAiModelStore`.

`LocalAiModelStore` obsługuje:

- status modelu,
- pobieranie modelu,
- zapis lokalny,
- weryfikację SHA256,
- usunięcie pliku przy błędnym hash.

### Obraz lokalnie

Ścieżka obrazu używa teraz:

- `OnDeviceImageAnalysisProvider`,
- `ILocalVisionInferenceEngine`,
- `LocalVisionInferenceEngine`.

Mock provider obrazu został usunięty z Core i Mobile. Provider obrazu wymaga pobranego i zweryfikowanego modelu `local-vision-v1`.

### Dźwięk lokalnie

Dodano rozpoznawanie dźwięku jako osobny moduł:

- `AudioAnalysisRequest`,
- `AudioAnalysisResult`,
- `AudioAnalysisProjectPolicy`,
- `AudioAnalysisPolicyService`,
- `IAudioAnalysisProvider`,
- `OnDeviceAudioAnalysisProvider`,
- `ILocalAudioInferenceEngine`,
- `LocalAudioInferenceEngine`,
- `AudioAnalysisService`.

Włączone projekty audio:

- `zmywarka-diagnosta`,
- `pies-trener-7dni`,
- `kot-bawi-sie`.

Provider dźwięku wymaga pobranego i zweryfikowanego modelu `local-audio-v1`.

### UI

Dodano:

- sekcję pobierania modeli lokalnych w `Settings.razor`,
- kartę `Local AI image` w `Categories.razor`,
- kartę `Local AI audio` w `Categories.razor`.

### Build profile per aplikacja

Dodano modele, serwis, raport i generator build profiles. Każdy projekt ma stabilny `ApplicationId`.

### UX katalogu, kategorii i quizu

Dodano wyszukiwarkę aplikacji, filtr typu doświadczenia, licznik projektów, postęp quizu, powrót do poprzedniego pytania, reset quizu i powrót z wyniku do quizu.

### Result navigation state

Dodano `ResultNavigationStateService`. Quiz zapisuje metadane dopasowania w serwisie stanu, a ekran wyniku odczytuje je z serwisu. Query string parser został jako fallback.

### Dane projektów

Dodano `reason` w źródle i runtime dla wszystkich 20 projektów.

Wyrównano `plama-ratownik` source/runtime.

## Znane ryzyka przed statusem produkcyjnym

- Pełna weryfikacja wymaga lokalnego uruchomienia testów lub przejścia workflow CI.
- Modele `local-vision-v1` i `local-audio-v1` mają profile, ale wymagają skonfigurowania `DownloadUrl` i `Sha256`.
- `LocalVisionInferenceEngine` i `LocalAudioInferenceEngine` są adapterami pod natywny runtime inferencji; trzeba podłączyć ONNX albo TFLite dla Android MAUI.

## Następne kroki techniczne

1. Wybrać ONNX albo TFLite jako runtime lokalnego AI.
2. Dodać pakiet runtime do `AppFactory.Mobile.csproj`.
3. Skonfigurować `DownloadUrl` i `Sha256` dla modeli.
4. Zaimplementować `LocalVisionInferenceEngine`.
5. Zaimplementować `LocalAudioInferenceEngine`.
6. Uruchomić `pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport` albo poczekać na workflow CI.

## Uwagi

Nie uruchamiałem lokalnie `dotnet test`, bo pracuję przez GitHub connector. Weryfikacja kompilacji wymaga lokalnego `dotnet test`, `pwsh ./tools/quality/run-quality-checks.ps1` albo przejścia workflow CI.
