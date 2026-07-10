# Status jakości projektów AppFactory

## Status produkcyjny

Aktualny status repo: **production-ready candidate**.

Kod zawiera komplet danych, scenariuszy, profili UI/UX i automatycznych gate'ów implementacji. Finalny status `production-ready` wymaga zielonego CI oraz wykonania scenariuszy na Androidzie.

## Zakres katalogu

Katalog obejmuje 20 projektów.

## UI/UX wszystkich aplikacji

Każdy projekt ma teraz dedykowany profil:

- ikonę,
- badge doświadczenia,
- własny hero title i opis,
- nagłówki kategorii, quizu i wyniku,
- typ widoku wyniku,
- etykiety akcji premium i zapisu,
- konfigurację safety, kopiowania lub narzędzi specjalnych.

Obsługiwane typy wyników:

- instrukcja,
- techniczna checklista,
- plan 7 dni,
- bajka,
- opis sprzedażowy,
- aktywność zwierzaka,
- checklista stylu,
- lekcja kreatywna,
- plan aranżacji,
- checklista pakowania,
- pomocnik craft,
- diagnostyka.

Wspólny design system obejmuje:

- sticky topbar,
- dolną nawigację,
- safe-area Android,
- responsywne siatki,
- gradientowe hero,
- większe cele dotykowe,
- progress quizu,
- kafle odpowiedzi,
- stany puste,
- karty bezpieczeństwa,
- warianty wyniku zależne od projektu,
- `prefers-reduced-motion`.

Raport:

```text
docs/quality/UI_UX_AUDIT.md
```

Gate:

```text
tests/AppFactory.Mobile.Tests/UiUxProductionTests.cs
```

## Scenariusze produkcyjne

Dla każdego projektu istnieje:

```text
projects/<projectId>/tests/production-scenarios.md
```

Łączne pokrycie:

- **20 projektów**,
- **5 scenariuszy na projekt**,
- **100 scenariuszy produkcyjnych**.

Test struktury:

```text
tests/AppFactory.Mobile.Tests/ProjectProductionScenariosTests.cs
```

Test osiągalności reguł:

```text
tests/AppFactory.Mobile.Tests/AllProjectRuleReachabilityTests.cs
```

## Audyt akcji i logiki biznesowej

Dodano:

```text
tests/AppFactory.Mobile.Tests/ScenarioImplementationAuditTests.cs
docs/quality/SCENARIO_IMPLEMENTATION_AUDIT.md
```

Każdy scenariusz jest mapowany na wymagane capabilities, między innymi:

- katalog i motyw,
- quiz i silnik reguł,
- wynik free/premium,
- ulubione i historia,
- języki PL/EN/UK,
- fallback,
- alternatywy,
- kopiowanie,
- Local AI image/audio,
- trwałość danych,
- safety,
- narzędzia projektu.

Test sprawdza dowody implementacji w faktycznych stronach, usługach, danych i politykach projektu.

## Naprawione luki scenariuszy

### Local AI

Dodano rzeczywisty wybór lokalnego zdjęcia i nagrania przez `LocalMediaInputService` oraz `LocalAiPanel`.

Sugestie AI są przenoszone do quizu przez `AiSuggestionStateService`. Quiz pokazuje przycisk `Użyj tej sugestii`; odpowiedź trafia do formularza dopiero po ręcznym zatwierdzeniu.

### Historia i ulubione

Wpisy przechowują pełną trasę wyniku:

- projekt,
- kategorię,
- wynik free,
- wynik premium.

Historia i ulubione pozwalają ponownie otworzyć zapisany wynik.

### Kopiowanie

Wspólna akcja kopiowania działa dla:

- `vinted-olx-opis`,
- `barber-translator`.

### Szydełko

Dodano:

- licznik rzędów,
- zwiększanie, zmniejszanie i reset,
- lokalny zapis przez `Preferences`,
- notatki robótki.

## Testy globalne i produkcyjne

Repo zawiera między innymi:

- `AllProjectsQualityTests.cs`,
- `RuleReasonsQualityTests.cs`,
- `ProductionReadinessTests.cs`,
- `ProjectProductionScenariosTests.cs`,
- `ScenarioImplementationAuditTests.cs`,
- `AllProjectRuleReachabilityTests.cs`,
- `UiUxProductionTests.cs`,
- `AiSuggestionWorkflowTests.cs`,
- `RuleEngineServiceTests.cs`,
- `MatchInfoParserTests.cs`,
- `ResultNavigationStateServiceTests.cs`,
- `BuildProfileServiceTests.cs`,
- `ImageAnalysisServiceTests.cs`,
- `AudioAnalysisServiceTests.cs`,
- `LocalAiModelStoreTests.cs`,
- `LocalAiInputTensorFactoryTests.cs`.

## Local AI on device

Runtime:

```text
Microsoft.ML.OnnxRuntime
```

Warstwa obejmuje downloader, SHA256, lokalny model store, tensor input, provider obrazu, provider audio, FilePicker oraz ręczne zatwierdzanie sugestii w quizie.

Aktualny kontrakt wejścia:

```text
input: float32[1,1,1,256]
```

Modele `local-vision-v1` i `local-audio-v1` wymagają skonfigurowania adresu pobierania, SHA256 i docelowych etykiet klas.

## Weryfikacja wymagana przed publikacją

1. Uruchomić:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport
```

2. Wykonać 100 scenariuszy na emulatorze lub urządzeniu Android.
3. Zapisać PASS/FAIL dla każdego scenariusza.
4. Sprawdzić FilePicker, klawiaturę, safe-area, systemowy back i długie tłumaczenia.
5. Naprawić wszystkie błędy krytyczne i wysokie.
6. Skonfigurować docelowe modele ONNX albo wyłączyć funkcje AI w buildzie publikacyjnym do czasu gotowości modeli.

## Uwagi

Nie uruchamiałem lokalnie `dotnet test`, ponieważ zmiany wykonuję przez GitHub connector. Status CI i wygląd na fizycznym urządzeniu nie zostały potwierdzone w tej sesji.
