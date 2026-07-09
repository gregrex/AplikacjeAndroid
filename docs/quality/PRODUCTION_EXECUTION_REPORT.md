# Raport wykonania checklisty produkcyjnej

## Zakres

Checklistę wykonano dla katalogu 20 projektów AppFactory:

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

## Wynik wykonania

| Obszar | Status | Dowód w repo |
| --- | --- | --- |
| Komplet źródeł projektu | DONE | `tests/AppFactory.Mobile.Tests/AllProjectsQualityTests.cs`, `tests/AppFactory.Mobile.Tests/ProductionReadinessTests.cs` |
| Runtime mirror | DONE | `tests/AppFactory.Mobile.Tests/AllProjectsQualityTests.cs`, `tools/quality/sync-runtime-packs.ps1` |
| `reason` dla każdej reguły | DONE | `tests/AppFactory.Mobile.Tests/RuleReasonsQualityTests.cs`, `tests/AppFactory.Mobile.Tests/ProductionReadinessTests.cs` |
| PL/EN/UK parytet wyników | DONE | `AllProjectsQualityTests.CatalogProjects_HaveLanguageParity`, `ProductionReadinessTests.EveryCatalogProject_HasRuleReasonsAndLanguageParity` |
| Marketing PL | DONE | `projects/<projectId>/marketing/store-listing.pl.md` dla każdego projektu |
| Manual QA | DONE | `projects/<projectId>/tests/manual-tests.md` dla każdego projektu |
| UX wyniku | DONE | `Result.razor`, `Quiz.razor`, `MatchInfoParser`, testy parsera i silnika reguł |
| Bezpieczny fallback reguł | DONE | `RuleEngineServiceTests`, `QUALITY_STATUS.md`, reguły bezpieczeństwa w projektach technicznych |
| CI jakości | DONE | `.github/workflows/quality-checks.yml` |
| Pełna weryfikacja testów | PENDING_EXTERNAL_RUN | Wymaga przejścia GitHub Actions albo lokalnego `dotnet test` |

## Wykonane poprawki po checklistcie

### `plama-ratownik`

Wyrównano projekt do wymogów produkcyjnych:

- source `rules.json` rozszerzony do pełnego zestawu runtime,
- source `results.pl.json` rozszerzony do pełnego zestawu wyników,
- dodano source `results.en.json`,
- dodano source `results.uk.json`.

### Automatyzacja checklisty

Dodano test:

```text
tests/AppFactory.Mobile.Tests/ProductionReadinessTests.cs
```

Test sprawdza:

- istnienie infrastruktury produkcyjnej,
- komplet danych source i runtime,
- obecność theme source/runtime,
- listing marketingowy PL,
- testy manualne,
- `reason` w source i runtime,
- parytet PL/EN/UK w source i runtime,
- zgodność ID reguł source/runtime,
- brak blokujących markerów ryzyka w `QUALITY_STATUS.md`.

## Aktualny status produkcyjny

Status kodu i danych w repo: **production-ready candidate**.

Nie oznaczam jeszcze jako final production, dopóki nie przejdzie jedno z poniższych:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport
```

albo workflow:

```text
Quality Checks
```

## Następne kroki

1. Poczekać na wynik GitHub Actions `Quality Checks`.
2. Jeśli testy przejdą, oznaczyć status jako `production-ready`.
3. Jeśli testy zwrócą błędy, naprawić pierwszy błąd danych lub kompilacji.
4. Po zielonym CI przygotować profile buildów osobnych aplikacji.
