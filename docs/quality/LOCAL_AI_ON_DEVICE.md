# Local AI on device

## Cel

Ten moduł dodaje lokalne rozpoznawanie obrazu i dźwięku na telefonie przez ONNX Runtime.

Aplikacja nie używa mockowego providera obrazu w DI. Produkcyjna ścieżka obrazu przechodzi przez `OnDeviceImageAnalysisProvider`.

## Runtime

Wybrany runtime:

```text
Microsoft.ML.OnnxRuntime
```

Pakiet został dodany do:

```text
src/AppFactory.Core/AppFactory.Core.csproj
src/AppFactory.Mobile/AppFactory.Mobile.csproj
```

Wspólny runner:

```text
OnnxModelRunner
```

uruchamia model ONNX przez `InferenceSession` i zwraca wektor score'ów.

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

## Wejście modelu v1

Do czasu dobrania docelowych modeli i preprocesorów obowiązuje neutralne wejście:

```text
input: float32[1,1,1,256]
```

`LocalAiInputTensorFactory` czyta lokalny plik obrazu lub audio i buduje znormalizowany tensor z bajtów pliku.

To jest działająca ścieżka techniczna ONNX, ale jakość rozpoznawania zależy od docelowego modelu i etykiet.

## Obraz

Ścieżka obrazu:

```text
ImageAnalysisService
OnDeviceImageAnalysisProvider
ILocalVisionInferenceEngine
LocalVisionInferenceEngine
OnnxModelRunner
```

`OnDeviceImageAnalysisProvider` wymaga pobranego i zweryfikowanego modelu `local-vision-v1`.

`LocalVisionInferenceEngine` uruchamia model ONNX i mapuje score'y na sugestie odpowiedzi dla projektów obrazowych.

## Dźwięk

Ścieżka dźwięku:

```text
AudioAnalysisService
OnDeviceAudioAnalysisProvider
ILocalAudioInferenceEngine
LocalAudioInferenceEngine
OnnxModelRunner
```

`OnDeviceAudioAnalysisProvider` wymaga pobranego i zweryfikowanego modelu `local-audio-v1`.

`LocalAudioInferenceEngine` uruchamia model ONNX i mapuje score'y na sugestie odpowiedzi dla projektów audio.

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

## Do finalnego rozpoznawania jakościowego

Żeby uzyskać dobre rozpoznawanie produkcyjne, trzeba jeszcze:

1. Podstawić realne modele ONNX zgodne z wejściem albo dopisać dedykowane preprocesory.
2. Skonfigurować `DownloadUrl` i `Sha256` w `LocalAiModelCatalogService`.
3. Dodać etykiety klas dla każdego modelu.
4. Dodać test integracyjny na małym modelu testowym.
5. Uruchomić testy na Androidzie, bo runtime działa na urządzeniu, a nie tylko w kodzie repo.
