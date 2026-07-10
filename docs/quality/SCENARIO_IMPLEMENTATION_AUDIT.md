# Audyt implementacji scenariuszy produkcyjnych

## Cel

Audyt odpowiada na dwa pytania dla każdego z 100 scenariuszy:

1. Czy istnieje wymagana akcja użytkownika w UI?
2. Czy istnieje logika biznesowa, która obsługuje tę akcję?

## Automatyczny audyt

Test:

```text
tests/AppFactory.Mobile.Tests/ScenarioImplementationAuditTests.cs
```

czyta wszystkie pliki:

```text
projects/<projectId>/tests/production-scenarios.md
```

Następnie analizuje treść każdego `SCN-01`–`SCN-05` i wykrywa wymagane możliwości.

## Sprawdzane możliwości

| Capability | Akcja UI | Logika biznesowa / dane |
| --- | --- | --- |
| `catalog` | wybór aplikacji | `ProjectCatalogService` |
| `theme` | grafika aktywnego projektu | source/runtime `theme.json` |
| `quiz` | wybór odpowiedzi i pokazanie wyniku | `Quiz.razor`, `RuleEngine.Match` |
| `rule-engine` | dopasowanie odpowiedzi | `RuleEngineService` |
| `result-data` | prezentacja wyniku | reguły i wyniki PL/EN/UK |
| `premium` | odblokowanie pełnej wersji | `MockAdService`, `UnlockPremium` |
| `favorites` | zapis i ponowne otwarcie | `Favorites.AddAsync`, `Favorites.razor` |
| `history` | zapis i ponowne otwarcie | `History.AddAsync`, `History.razor` |
| `localization` | wybór języka | `LanguageService`, wyniki PL/EN/UK |
| `fallback` | wynik bez reguły szczegółowej | globalna reguła `categoryId = *`, puste `when` |
| `alternatives` | wybór alternatywy | silnik alternatyw i `SelectAlternative` |
| `clipboard` | kopiowanie treści | `ClipboardExportService`, `ProjectResultView` |
| `image-ai` | wybór zdjęcia | `LocalAiPanel`, `LocalMediaInputService`, provider ONNX |
| `audio-ai` | wybór nagrania | `LocalAiPanel`, `AudioAnalysisService`, provider ONNX |
| `project-tools` | licznik i notatki | `ProjectTools`, `ProjectToolStateService` |
| `persistence` | zachowanie stanu | lokalne usługi historii, ulubionych i narzędzi |
| `safety` | ostrzeżenia i blokady | profile safety oraz `warnings` w wynikach |

## Zasada walidacji

Każdy scenariusz zawsze wymaga:

- obecności projektu w katalogu,
- motywu source i runtime,
- działającego quizu,
- silnika reguł,
- danych wynikowych.

Dodatkowe wymagania są wykrywane z treści scenariusza, np. słowa `premium`, `ulubione`, `historia`, `ONNX`, `nagranie`, `kopiuj`, `fallback` lub `licznik rzędów`.

## Osiągalność logiki biznesowej

Osobny test:

```text
tests/AppFactory.Mobile.Tests/AllProjectRuleReachabilityTests.cs
```

sprawdza każdą regułę niezależnie:

- tworzy odpowiedzi z jej `when`,
- uruchamia silnik reguł,
- wymaga dopasowania tej reguły,
- sprawdza wynik free i premium,
- sprawdza obecność obu wyników w PL, EN i UK,
- sprawdza osiągalność każdej kategorii.

## Aktualny status

Akcje i logika zostały podłączone w kodzie oraz objęte gate'ami testowymi.

Status nie oznacza jeszcze `100/100 PASS` na Androidzie. Finalna weryfikacja wymaga wykonania `SCENARIO_EXECUTION_TRACKER.md` na emulatorze lub urządzeniu i zapisania wyniku każdego scenariusza.
