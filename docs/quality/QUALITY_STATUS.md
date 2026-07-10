# Status jakości projektów AppFactory

## Status produkcyjny

Aktualny status repo: **production-ready candidate**.

Kod zawiera komplet danych, scenariuszy, profili UI/UX i automatycznych gate'ów implementacji. Finalny status `production-ready` wymaga zielonego CI oraz wykonania scenariuszy na Androidzie.

## Zakres katalogu

Katalog obejmuje 20 projektów.

## UI/UX wszystkich aplikacji

Każdy projekt ma dedykowany profil:

- ikonę,
- badge doświadczenia,
- własny hero title i opis,
- nagłówki kategorii, quizu i wyniku,
- typ widoku wyniku,
- etykiety akcji premium i zapisu,
- konfigurację safety, kopiowania lub narzędzi specjalnych.

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

Testy:

```text
tests/AppFactory.Mobile.Tests/ProjectProductionScenariosTests.cs
tests/AppFactory.Mobile.Tests/AllProjectRuleReachabilityTests.cs
tests/AppFactory.Mobile.Tests/ScenarioImplementationAuditTests.cs
```

## Audyt akcji i logiki biznesowej

Każdy scenariusz jest mapowany na wymagane capabilities:

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

Historia i ulubione:

- zapisują dane lokalnie przez `Preferences`,
- pozostają po restarcie aplikacji,
- pozwalają ponownie otworzyć wynik,
- obsługują czyszczenie list,
- ulubione obsługują usunięcie pojedynczej pozycji.

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

Nie uruchamiałem lokalnie `dotnet test`, ponieważ zmiany wykonuję przez GitHub connector. Dla ostatniego sprawdzonego commita GitHub nie zwrócił żadnych statusów CI, więc kompilacja i testy nie są jeszcze potwierdzone. Wygląd na fizycznym urządzeniu również nie został potwierdzony w tej sesji.
