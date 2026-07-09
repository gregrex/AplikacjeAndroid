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

Dodano dokumenty jakości, produkcji, build profiles oraz `docs/quality/IMAGE_ANALYSIS_V1.md`.

### CI

Dodano `.github/workflows/quality-checks.yml`.

### Testy globalne i produkcyjne

Dodano:

- `tests/AppFactory.Mobile.Tests/AllProjectsQualityTests.cs`,
- `tests/AppFactory.Mobile.Tests/RuleReasonsQualityTests.cs`,
- `tests/AppFactory.Mobile.Tests/ProductionReadinessTests.cs`,
- `tests/AppFactory.Mobile.Tests/ResultNavigationStateServiceTests.cs`,
- `tests/AppFactory.Mobile.Tests/BuildProfileServiceTests.cs`,
- `tests/AppFactory.Mobile.Tests/ImageAnalysisServiceTests.cs`.

Testy sprawdzają dane projektów, runtime, parytet języków, `reason`, profile buildów, serwis stanu wyniku oraz Image Analysis v1.

### Build profile per aplikacja

Dodano modele, serwis, raport i generator build profiles. Każdy projekt ma stabilny `ApplicationId` w formacie:

```text
pl.gbcom.appfactory.<projectIdBezMyślników>
```

### Image Analysis v1

Dodano fundament analizy obrazu:

- modele request/result/suggestion/policy,
- `ImageAnalysisPolicyService`,
- `IImageAnalysisProvider`,
- `MockImageAnalysisProvider`,
- `ImageAnalysisService`,
- testy `ImageAnalysisServiceTests`,
- dokument `IMAGE_ANALYSIS_V1.md`,
- informację w UI kategorii dla projektów z włączoną analizą obrazu.

Włączone projekty:

- `plama-ratownik`,
- `pokoj-makeover`,
- `rysunek-coach`,
- `outfit-coach`,
- `fryzury-proste`,
- `barber-translator`,
- `zmywarka-diagnosta`,
- `silikon-fuga-fix`.

Image Analysis v1 używa mock providera. Wynik analizy jest tylko podpowiedzią do formularza i wymaga ręcznego potwierdzenia przez użytkownika.

### UX katalogu, kategorii i quizu

Dodano:

- wyszukiwarkę aplikacji w katalogu,
- filtr typu doświadczenia,
- licznik widocznych projektów,
- kartę `Image Analysis v1` dla projektów obsługujących obraz,
- postęp quizu,
- powrót do poprzedniego pytania,
- reset quizu,
- powrót z wyniku do quizu.

### Result navigation state

Dodano `ResultNavigationStateService`. Quiz zapisuje metadane dopasowania w serwisie stanu, a ekran wyniku odczytuje je z serwisu. Query string parser został jako fallback.

### Rule Engine v2 i ekran wyniku

Silnik reguł zwraca teraz `Score`, `Reason`, `MatchedConditions`, `AlternativeRuleIds` i `AlternativePremiumResultIds`.

Ekran wyniku pokazuje wyjaśnienie wyniku, dopasowane odpowiedzi i alternatywne rekomendacje po odblokowaniu premium.

### Dane projektów

Dodano `reason` w źródle i runtime dla wszystkich 20 projektów.

Wyrównano `plama-ratownik`:

- source `rules.json`,
- source `results.pl.json`,
- source `results.en.json`,
- source `results.uk.json`.

## Znane ryzyka przed statusem produkcyjnym

- Pełna weryfikacja wymaga lokalnego uruchomienia testów lub przejścia workflow CI.
- Image Analysis v1 ma mock providera. Realny provider wymaga osobnego pakietu integracyjnego.

## Następne kroki techniczne

1. Poczekać na wynik GitHub Actions `Quality Checks` albo uruchomić lokalnie `pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport`.
2. Naprawić ewentualne błędy danych ujawnione przez globalny test.
3. Po zielonym CI zmienić status z `production-ready candidate` na `production-ready`.
4. Rozwinąć profile buildów o rzeczywiste flavor/pipeline per aplikacja.
5. Zastąpić `MockImageAnalysisProvider` realnym providerem.
6. Dodać testy UI/snapshoty dla `Home`, `Categories`, `Quiz` i `Result`.

## Uwagi

Nie uruchamiałem lokalnie `dotnet test`, bo pracuję przez GitHub connector. Weryfikacja kompilacji wymaga lokalnego `dotnet test`, `pwsh ./tools/quality/run-quality-checks.ps1` albo przejścia workflow CI.
