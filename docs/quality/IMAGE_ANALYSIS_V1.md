# Image Analysis v1

## Cel

Image Analysis v1 dodaje wspólną warstwę lokalnej analizy obrazu dla projektów AppFactory.

Wersja v1 używa `OnDeviceImageAnalysisProvider`. Provider nie zwraca sztucznego wyniku: wymaga pobranego i zweryfikowanego lokalnego modelu obrazu. Jeśli model nie jest gotowy, analiza jest blokowana czytelnym komunikatem.

## Zakres v1

Dodano:

- `ImageAnalysisRequest`,
- `ImageAnalysisResult`,
- `ImageAnswerSuggestion`,
- `ImageAnalysisProjectPolicy`,
- `ImageAnalysisPolicyService`,
- `IImageAnalysisProvider`,
- `OnDeviceImageAnalysisProvider`,
- `ILocalVisionInferenceEngine`,
- `LocalVisionInferenceEngine`,
- `ImageAnalysisService`.

Te elementy istnieją w:

```text
src/AppFactory.Core
src/AppFactory.Mobile
```

## Projekty z włączoną analizą obrazu

W v1 analiza obrazu jest włączona dla:

- `plama-ratownik`,
- `pokoj-makeover`,
- `rysunek-coach`,
- `outfit-coach`,
- `fryzury-proste`,
- `barber-translator`,
- `zmywarka-diagnosta`,
- `silikon-fuga-fix`.

## Projekty safety-sensitive

Projekty wymagające ostrożniejszego UX:

- `plama-ratownik`,
- `zmywarka-diagnosta`,
- `silikon-fuga-fix`.

Dla tych projektów wynik analizy obrazu musi zawierać ostrzeżenie, że zdjęcie jest tylko podpowiedzią i nie zastępuje oceny ryzyka.

## Walidacja wejścia

`ImageAnalysisService` odrzuca:

- projekt bez włączonej analizy obrazu,
- pusty plik,
- plik większy niż limit projektu,
- typ pliku spoza listy akceptowanych MIME.

Domyślne typy:

```text
image/jpeg
image/png
image/webp
```

Domyślny limit:

```text
5 MB
```

## Model lokalny

Model obrazu jest opisany w `LocalAiModelCatalogService` jako:

```text
local-vision-v1
```

Do działania produkcyjnego trzeba skonfigurować:

- `DownloadUrl`,
- `Sha256`,
- rozmiar modelu,
- natywną implementację `ILocalVisionInferenceEngine` dla Android runtime.

## UX

Na stronie kategorii projektów z włączoną analizą obrazu pokazuje się karta `Local AI image`.

Komunikat mówi jasno, że:

- analiza obrazu jest opcjonalna,
- wynik jest podpowiedzią do formularza,
- użytkownik nadal potwierdza odpowiedzi ręcznie,
- w projektach safety-sensitive zdjęcie nie zastępuje oceny ryzyka.

## Testy

Dodano:

```text
tests/AppFactory.Mobile.Tests/ImageAnalysisServiceTests.cs
```

Testy sprawdzają:

- listę projektów z włączoną analizą obrazu,
- odrzucenie projektu bez image analysis,
- odrzucenie złego typu pliku,
- odrzucenie za dużego pliku,
- blokadę, gdy lokalny model nie jest skonfigurowany albo zweryfikowany.

## Następny krok

Podłączyć natywny runtime inferencji obrazu w `LocalVisionInferenceEngine` oraz skonfigurować URL i SHA256 modelu `local-vision-v1`.
