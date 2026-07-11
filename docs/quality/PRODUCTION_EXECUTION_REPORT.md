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
| Audyt akcji i logiki scenariuszy | DONE_IN_CODE | `ScenarioImplementationAuditTests.cs` |
| Osiągalność reguł i kategorii | DONE_IN_CODE | `AllProjectRuleReachabilityTests.cs` |
| Dedykowany profil UI dla 20 projektów | DONE_IN_CODE | `ProjectUiProfileService`, `UiUxProductionTests.cs` |
| Wspólny design system | DONE_IN_CODE | `app.css`, `MainLayout.razor` |
| UX katalogu i quizu | DONE_IN_CODE | `Home.razor`, `Categories.razor`, `Quiz.razor` |
| UX wyniku | DONE_IN_CODE | `Result.razor`, `ProjectResultView.razor` |
| Lokalna baza SQLite | DONE_IN_CODE | `AppFactory.Persistence`, `AppDatabase.cs` |
| Testy integracyjne SQLite | DONE_IN_CODE | `AppDatabaseTests.cs`, `LocalDatabaseProductionTests.cs` |
| Migracja historii i ulubionych | DONE_IN_CODE | `HistoryService`, `FavoritesService` |
| Local AI wybór pliku | DONE_IN_CODE | `LocalMediaInputService`, `LocalAiPanel` |
| Ręczne zatwierdzanie sugestii AI | DONE_IN_CODE | `AiSuggestionStateService`, `AiSuggestionWorkflowTests.cs` |
| Ponowne otwieranie historii i ulubionych | DONE_IN_CODE | rozszerzone wpisy i strony zapisu |
| Licznik rzędów i notatki | DONE_IN_CODE | `ProjectTools.razor`, `ProjectToolStateService` |
| Bezpieczny fallback reguł | DONE_IN_CODE | `RuleEngineServiceTests.cs`, scenariusze domenowe |
| Local AI image/audio | CANDIDATE | ONNX Runtime, downloader, provider obrazu i audio |
| Lokalny runner testów | DONE | `run-local-test-plan.ps1`, `LOCAL_TEST_PLAN.md` |
| CI jakości | CONFIGURED | `.github/workflows/quality-checks.yml` |
| Kompilacja i testy | PENDING_EXTERNAL_RUN | GitHub Actions albo lokalny `dotnet test` |
| Migracja SQLite na Androidzie | NOT_RUN | wymaga instalacji bez czyszczenia danych |
| Wykonanie 100 scenariuszy na Androidzie | NOT_RUN | emulator lub urządzenie |
| Audyt screenshotów na Androidzie | NOT_RUN | emulator lub urządzenie |

## Lokalna baza danych

Dodano osobny projekt:

```text
src/AppFactory.Persistence/AppFactory.Persistence.csproj
```

SQLite przechowuje:

- historię wyników,
- ulubione wyniki,
- wersję schematu.

Historia jest deduplikowana, sortowana i ograniczona do 100 wpisów. Ulubione są deduplikowane oraz można je usuwać pojedynczo lub czyścić w całości.

`HistoryService` i `FavoritesService` wykonują jednorazową migrację dotychczasowych list JSON z `Preferences`.

Dokumentacja:

```text
docs/quality/LOCAL_DATABASE.md
```

## Plan lokalnych testów

Gotowy plan:

```text
docs/quality/LOCAL_TEST_PLAN.md
```

Runner:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Runner wykonuje:

- kontrolę SDK i workloadów,
- restore,
- wszystkie testy,
- osobny przebieg testów SQLite,
- Android Debug build,
- opcjonalny Android Release build,
- raport jakości,
- kontrolę `adb devices`,
- zapis logów i podsumowania w `artifacts/local-test/<timestamp>`.

## UI/UX

Każdy projekt ma dedykowane:

- ikonę,
- badge,
- tekst hero,
- nagłówki przepływu,
- typ prezentacji wyniku,
- etykiety akcji,
- konfigurację safety, kopiowania i narzędzi.

Pełny raport:

```text
docs/quality/UI_UX_AUDIT.md
```

## Sprawdzenie scenariuszy

Dla każdego scenariusza test `ScenarioImplementationAuditTests.cs` wykrywa wymagane możliwości i sprawdza dowody w kodzie, danych albo politykach projektu.

Osobny test `AllProjectRuleReachabilityTests.cs` uruchamia każdą regułę z odpowiedziami utworzonymi z jej `when` i sprawdza wyniki free/premium w PL, EN i UK.

## Aktualny status produkcyjny

Status kodu i danych: **production-ready candidate**.

Nie oznaczam repo jako final `production-ready`, ponieważ nie ma w tej sesji potwierdzenia zielonego builda/testów, migracji SQLite na Androidzie, screenshotów ani wykonania 100 scenariuszy.

## Następne kroki

1. Uruchomić:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

2. Naprawić pierwszy błąd kompilacji lub testu.
3. Wykonać smoke test.
4. Wykonać test nowej bazy i migracji `Preferences -> SQLite`.
5. Wykonać 100 scenariuszy oraz audyt screenshotów.
6. Zapisać PASS/FAIL i numer defektu w `SCENARIO_EXECUTION_TRACKER.md`.
7. Po wyniku 100/100 PASS i zielonym CI zmienić status na `production-ready`.
