# Local AI on device

## Cel

Ten moduł przygotowuje lokalne rozpoznawanie obrazu i dźwięku na telefonie.

Aplikacja nie używa już mockowego providera obrazu w DI. Produkcyjna ścieżka obrazu przechodzi przez `OnDeviceImageAnalysisProvider`.

## Modele lokalne

Profile modeli są w:

```text
LocalAiModelCatalogService
```

Aktualne profile:

| ModelId | Modality | File |
| --- | --- | --- |
| `local-vision-v1` | `image` | `local-vision-v1.onnx` |
| `local-audio-v1` | `audio` | `local-audio-v1.onnx` |

## Pobieranie modeli

Pobieranie obsługuje:

```text
LocalAiModelStore
```

Funkcje:

- sprawdza konfigurację modelu,
- pobiera model z URL,
- zapisuje model lokalnie,
- weryfikuje SHA256,
- usuwa plik, jeśli hash się nie zgadza.

UI pobierania jest w:

```text
src/AppFactory.Mobile/Pages/Settings.razor
```

## Obraz

Ścieżka obrazu:

```text
ImageAnalysisService
OnDeviceImageAnalysisProvider
ILocalVisionInferenceEngine
LocalVisionInferenceEngine
```

`OnDeviceImageAnalysisProvider` wymaga pobranego i zweryfikowanego modelu `local-vision-v1`.

`LocalVisionInferenceEngine` jest miejscem na natywną implementację Android ONNX/TFLite. Do czasu podłączenia runtime nie zwraca rozpoznania.

## Dźwięk

Ścieżka dźwięku:

```text
AudioAnalysisService
OnDeviceAudioAnalysisProvider
ILocalAudioInferenceEngine
LocalAudioInferenceEngine
```

`OnDeviceAudioAnalysisProvider` wymaga pobranego i zweryfikowanego modelu `local-audio-v1`.

`LocalAudioInferenceEngine` jest miejscem na natywną implementację Android ONNX/TFLite. Do czasu podłączenia runtime nie zwraca rozpoznania.

## Projekty audio

Analiza dźwięku jest włączona dla:

- `zmywarka-diagnosta`,
- `pies-trener-7dni`,
- `kot-bawi-sie`.

## Testy

Dodano:

- `ImageAnalysisServiceTests`,
- `AudioAnalysisServiceTests`,
- `LocalAiModelStoreTests`.

Testy sprawdzają polityki, walidację plików, blokadę bez modelu oraz pobieranie i weryfikację SHA256.

## Do finalnej inferencji

Żeby uruchomić prawdziwe rozpoznawanie na telefonie, trzeba jeszcze:

1. Wybrać runtime ONNX albo TFLite dla Android MAUI.
2. Dodać pakiet runtime do `AppFactory.Mobile.csproj`.
3. Skonfigurować `DownloadUrl` i `Sha256` w `LocalAiModelCatalogService`.
4. Zaimplementować `LocalVisionInferenceEngine`.
5. Zaimplementować `LocalAudioInferenceEngine`.
6. Dodać test integracyjny na małym modelu testowym.
