# Status jakości projektów AppFactory

## Data kontroli

Etap jakościowy rozpoczęty po domknięciu 20 projektów MVP.

## Zakres katalogu

Katalog aplikacji obejmuje 20 projektów:

1. `plama-ratownik`
2. `kolek-dobieracz`
3. `pies-trener-7dni`
4. `bajka-z-rysunku`
5. `vinted-olx-opis`
6. `kot-bawi-sie`
7. `barber-translator`
8. `outfit-coach`
9. `domfix`
10. `fryzury-proste`
11. `rysunek-coach`
12. `bukietownik`
13. `pokoj-makeover`
14. `pakowanie-paczek`
15. `silikon-fuga-fix`
16. `szydelko-pomocnik`
17. `chleb-zakwas-coach`
18. `zmywarka-diagnosta`
19. `krawat-garnitur-coach`
20. `router-wifi-diagnosta`

## Wykonane w etapie jakościowym

### Dokumentacja

Dodano:

- `docs/quality/PLAN_DOPRACOWANIA_PROJEKTOW.md`,
- `docs/quality/QUALITY_STATUS.md`.

### Testy globalne

Dodano:

- `tests/AppFactory.Mobile.Tests/AllProjectsQualityTests.cs`.

Test globalny sprawdza:

- kompletność paczek źródłowych,
- kompletność paczek runtime,
- obecność `theme.json`,
- obecność listingów marketingowych,
- obecność testów manualnych,
- przejście przez wspólny `DataPackValidationService`,
- parytet językowy PL/EN/UK,
- zgodność strukturalną runtime względem źródła.

## Definicja MVP-ready po dopracowaniu

Projekt jest uznawany za MVP-ready, jeśli ma:

- źródłowy `data` pack,
- runtime pack w `wwwroot`,
- PL/EN/UK z tymi samymi `resultId`,
- theme źródłowy i runtime,
- manual tests,
- listing marketingowy,
- wpis w `ProjectCatalogService`,
- wynik przechodzący przez globalne testy jakości.

## Następne kroki techniczne

1. Dodać skrypt synchronizacji runtime.
2. Dodać skrypt raportujący brakujące pliki.
3. Rozszerzyć `DataPackValidationService` o walidację `app.json` i `theme.json`.
4. Dodać raport generatora do `docs/quality`.
5. Po lokalnym uruchomieniu `dotnet test` naprawić wszystkie realne błędy kompilacji lub danych.

## Uwagi

Testy globalne zostały dodane w repo, ale nie były uruchamiane lokalnie w tym trybie pracy. Weryfikacja kompilacji wymaga lokalnego `dotnet test` albo CI.
