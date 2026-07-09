# Image Analysis v1

## Cel

Image Analysis v1 dodaje wspólną, bezpieczną warstwę analizy obrazu dla projektów AppFactory.

Wersja v1 nie wysyła zdjęć do realnego dostawcy AI. Używa `MockImageAnalysisProvider`, żeby przygotować architekturę, testy i UX bez ryzyka prywatności oraz bez zależności od chmury.

## Zakres v1

Dodano:

- `ImageAnalysisRequest`,
- `ImageAnalysisResult`,
- `ImageAnswerSuggestion`,
- `ImageAnalysisProjectPolicy`,
- `ImageAnalysisPolicyService`,
- `IImageAnalysisProvider`,
- `MockImageAnalysisProvider`,
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

## UX

Na stronie kategorii projektów z włączoną analizą obrazu pokazuje się karta `Image Analysis v1`.

Komunikat mówi jasno, że:

- analiza obrazu jest opcjonalna,
- wersja v1 używa mock providera,
- użytkownik nadal potwierdza odpowiedzi ręcznie,
- w projektach safety-sensitive zdjęcie nie zastępuje oceny ryzyka.

## Zasada produktu

Analiza obrazu ma sugerować odpowiedzi do formularza, ale nie może automatycznie decydować za użytkownika.

To jest szczególnie ważne w projektach technicznych, domowych i safety-sensitive.

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
- mock sugestie dla projektu z włączoną analizą,
- warning dla projektu safety-sensitive.

## Następny krok

Po zielonym CI można dodać realnego providera:

```text
CloudImageAnalysisProvider
OnDeviceImageAnalysisProvider
```

Provider produkcyjny musi spełnić ten sam kontrakt `IImageAnalysisProvider` i przechodzić te same testy bezpieczeństwa.
