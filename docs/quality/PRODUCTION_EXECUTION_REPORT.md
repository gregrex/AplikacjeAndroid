# Raport wykonania checklisty produkcyjnej

## Zakres

Checklistę wykonano dla katalogu 20 projektów AppFactory.

## Wynik wykonania

| Obszar | Status | Dowód w repo |
| --- | --- | --- |
| Komplet źródeł projektu | DONE | `AllProjectsQualityTests.cs`, `ProductionReadinessTests.cs` |
| Runtime mirror | DONE | `AllProjectsQualityTests.cs`, `sync-runtime-packs.ps1` |
| `reason` dla każdej reguły | DONE | `RuleReasonsQualityTests.cs` |
| PL/EN/UK parytet wyników | DONE | `AllProjectsQualityTests.cs`, `ProductionReadinessTests.cs` |
| Marketing PL | DONE | `projects/<projectId>/marketing/store-listing.pl.md` |
| Manual QA | DONE | `projects/<projectId>/tests/manual-tests.md` |
| Pięć scenariuszy na projekt | DONE | `projects/<projectId>/tests/production-scenarios.md` |
| Łącznie 100 scenariuszy | DONE | `PROJECT_SCENARIO_COVERAGE.md` |
| Walidacja struktury scenariuszy | DONE | `ProjectProductionScenariosTests.cs` |
| Osiągalność reguł i kategorii | DONE_IN_CODE | `AllProjectRuleReachabilityTests.cs` |
| UX wyniku | DONE | `Result.razor`, `Quiz.razor`, `MatchInfoParser` |
| Bezpieczny fallback reguł | DONE_IN_CODE | `RuleEngineServiceTests.cs`, scenariusze domenowe |
| Local AI image/audio | CANDIDATE | ONNX Runtime, downloader, provider obrazu i audio |
| CI jakości | CONFIGURED | `.github/workflows/quality-checks.yml` |
| Kompilacja i testy | PENDING_EXTERNAL_RUN | Wymaga GitHub Actions albo lokalnego `dotnet test` |
| Wykonanie 100 scenariuszy na Androidzie | NOT_RUN | Wymaga emulatora lub urządzenia |

## Scenariusze produkcyjne

Dla każdego z 20 projektów dodano plik:

```text
projects/<projectId>/tests/production-scenarios.md
```

Każdy plik zawiera dokładnie:

- `SCN-01`,
- `SCN-02`,
- `SCN-03`,
- `SCN-04`,
- `SCN-05`.

Każdy scenariusz ma cel, kroki, oczekiwany wynik i opis pokrycia.

Automatyczny test `ProjectProductionScenariosTests.cs` sprawdza strukturę wszystkich 100 scenariuszy. Projekty obrazu muszą zawierać scenariusz `local-vision-v1` + ONNX, a projekty audio scenariusz `local-audio-v1` + ONNX.

## Sprawdzenie pełnego oprogramowania reguł

Dodano:

```text
tests/AppFactory.Mobile.Tests/AllProjectRuleReachabilityTests.cs
```

Test:

- buduje odpowiedzi z `when` każdej reguły,
- uruchamia `RuleEngineService`,
- wymaga dopasowania reguły,
- porównuje wynik free i premium,
- sprawdza obecność wyników w PL, EN i UK,
- sprawdza, czy każda kategoria ma osiągalną regułę albo globalny fallback.

## Aktualny status produkcyjny

Status kodu i danych: **production-ready candidate**.

Nie oznaczam repo jako final `production-ready`, ponieważ nie mam potwierdzenia zielonego builda/testów ani wykonania scenariuszy na Androidzie.

## Następne kroki

1. Uruchomić:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport
```

2. Naprawić błędy kompilacji lub testów.
3. Wykonać 100 scenariuszy na emulatorze lub urządzeniu.
4. Zapisać PASS/FAIL i numer defektu dla każdego nieudanego scenariusza.
5. Po wyniku 100/100 PASS zmienić status na `production-ready`.
