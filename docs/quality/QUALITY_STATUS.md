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
- `docs/quality/QUALITY_STATUS.md`,
- `docs/quality/RULE_ENGINE_V2.md`,
- `tools/quality/README.md`.

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
- walidację `app.json`,
- walidację `theme.json`,
- parytet językowy PL/EN/UK,
- zgodność strukturalną runtime względem źródła.

### Walidator

Rozszerzono:

- `src/AppFactory.Core/Services/DataPackValidationService.cs`.

Walidator sprawdza teraz:

- puste `appId`, `appName`, `defaultLanguage`, `supportedLanguages`,
- zgodność `appId` z katalogiem projektu,
- podstawowe pola `theme.json`,
- puste ID kategorii, pytań, reguł i wyników,
- puste `nameKey`, `textKey`, `icon`, `labelKey`,
- duplikaty opcji pytań,
- reguły wskazujące brakujące pytania,
- reguły używające wartości spoza opcji pytania,
- reguły wskazujące brakujące wyniki.

### Narzędzia

Dodano:

- `tools/quality/sync-runtime-packs.ps1`,
- `tools/quality/write-project-quality-report.ps1`,
- `tools/quality/run-quality-checks.ps1`.

Skrypty pozwalają lokalnie:

- zsynchronizować runtime z `projects`,
- wygenerować markdownowy raport jakości,
- uruchomić testy jakości,
- uruchomić testy po synchronizacji runtime i wygenerowaniu raportu.

### Rule Engine v2 i ekran wyniku

Rozszerzono:

- `src/AppFactory.Core/Models/ProjectDefinitions.cs`,
- `src/AppFactory.Mobile/Models/ProjectDefinitions.cs`,
- `src/AppFactory.Core/Services/RuleEngineService.cs`,
- `src/AppFactory.Mobile/Services/RuleEngineService.cs`,
- `src/AppFactory.Core/Models/MatchInfo.cs`,
- `src/AppFactory.Mobile/Models/MatchInfo.cs`,
- `src/AppFactory.Core/Services/MatchInfoParser.cs`,
- `src/AppFactory.Mobile/Services/MatchInfoParser.cs`,
- `src/AppFactory.Mobile/Pages/Quiz.razor`,
- `src/AppFactory.Mobile/Pages/Result.razor`,
- `tests/AppFactory.Mobile.Tests/RuleEngineServiceTests.cs`,
- `tests/AppFactory.Mobile.Tests/MatchInfoParserTests.cs`,
- `tests/AppFactory.Mobile.Tests/RuleReasonsQualityTests.cs`.

Silnik reguł zwraca teraz:

- `Score`,
- `Reason`,
- `MatchedConditions`,
- `AlternativeRuleIds`,
- `AlternativePremiumResultIds`.

Ekran wyniku pokazuje teraz:

- sekcję `Dlaczego taki wynik?`,
- regułę i punkty dopasowania,
- dopasowane odpowiedzi,
- alternatywne rekomendacje po odblokowaniu premium,
- przełączanie aktywnej rekomendacji premium na jedną z alternatyw.

Parsowanie metadanych wyniku zostało wyciągnięte z `Result.razor` do `MatchInfoParser`, dzięki czemu jest testowalne jednostkowo.

Poprawiono też fallback: reguła domyślna musi mieć `categoryId = *` oraz puste `when`.

### Projekty z pełnymi uzasadnieniami reguł

Dodano `reason` w źródle i runtime dla:

- `router-wifi-diagnosta`,
- `zmywarka-diagnosta`,
- `krawat-garnitur-coach`,
- `pakowanie-paczek`,
- `silikon-fuga-fix`.

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

1. Uruchomić lokalnie `pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport`.
2. Naprawić ewentualne błędy danych ujawnione przez globalny test.
3. Dodać `reason` do kolejnych projektów.
4. Rozważyć przeniesienie pełnego stanu dopasowania z query string do wspólnego serwisu stanu nawigacji.
5. Dodać osobny test dla logiki wyboru alternatywy, gdy UI zostanie przeniesiony do komponentu.

## Uwagi

Testy globalne, walidator, skrypty, Rule Engine v2, parser metadanych wyniku, ekran wyjaśnień i wybór alternatywnych rekomendacji zostały dodane w repo. Nie były uruchamiane lokalnie w tym trybie pracy. Weryfikacja kompilacji wymaga lokalnego `dotnet test`, `pwsh ./tools/quality/run-quality-checks.ps1` albo CI.
