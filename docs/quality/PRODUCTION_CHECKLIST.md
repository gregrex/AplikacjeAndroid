# Checklist produkcyjny AppFactory

Ten dokument opisuje minimalne warunki przejścia projektu z MVP do statusu produkcyjnego.

## 1. Dane projektu

Każdy projekt musi mieć komplet źródeł:

```text
projects/<projectId>/data/app.json
projects/<projectId>/data/categories.json
projects/<projectId>/data/questions.json
projects/<projectId>/data/rules.json
projects/<projectId>/data/results.pl.json
projects/<projectId>/data/results.en.json
projects/<projectId>/data/results.uk.json
projects/<projectId>/theme.json
```

Wymagania:

- `appId` zgodny z katalogiem projektu,
- unikalne ID kategorii, pytań, reguł i wyników,
- każda reguła wskazuje istniejący wynik free i premium,
- każda reguła ma pole `reason`,
- PL/EN/UK mają ten sam zestaw `resultId`,
- każdy wynik ma `title`, `summary` i przynajmniej jeden krok.

## 2. Runtime mirror

Każdy projekt musi mieć komplet runtime:

```text
src/AppFactory.Mobile/wwwroot/projects/<projectId>/app.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/categories.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/questions.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/rules.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/results.pl.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/results.en.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/results.uk.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/theme.json
```

Runtime powinien być synchronizowany z `projects` poleceniem:

```powershell
pwsh ./tools/quality/sync-runtime-packs.ps1
```

## 3. Testy jakości

Podstawowy gate:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport
```

Pełna lokalna sesja:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Testy muszą przejść bez błędów. Logi lokalnej sesji trafiają do:

```text
artifacts/local-test/<timestamp>
```

Szczegółowy plan:

```text
docs/quality/LOCAL_TEST_PLAN.md
```

Workflow GitHub Actions:

```text
.github/workflows/quality-checks.yml
```

uruchamia testy jakości na push, pull request i ręczne wywołanie.

## 4. Lokalna baza danych

Rosnące kolekcje muszą korzystać z SQLite:

- historia wyników,
- ulubione wyniki,
- wersja schematu.

Małe ustawienia mogą pozostać w `Preferences`.

Wymagania:

- schemat ma jawny numer wersji,
- baza inicjalizuje się przy pierwszym użyciu,
- historia jest sortowana i ograniczona do 100 wpisów,
- ulubione są deduplikowane,
- istnieje migracja wcześniejszych danych JSON z `Preferences`,
- testy `AppDatabaseTests` przechodzą,
- migracja została sprawdzona na urządzeniu bez czyszczenia danych.

Dokumentacja:

```text
docs/quality/LOCAL_DATABASE.md
```

## 5. Logi i diagnostyka

Aplikacja musi zapisywać lokalne logi JSONL z:

- czasem UTC,
- identyfikatorem sesji,
- poziomem i kategorią,
- `EventId`,
- wiadomością,
- typem i stack trace wyjątku.

Polityka produkcyjna:

- Debug loguje od poziomu `Debug`,
- Release loguje od poziomu `Information`,
- retencja wynosi 7 dni,
- maksymalnie 12 plików,
- maksymalnie 2 MB na plik,
- tokeny, sekrety, hasła i e-maile są maskowane,
- logi nie są wysyłane automatycznie,
- eksport wymaga ręcznej akcji użytkownika.

Aplikacja musi rejestrować minimum:

- start i wersję buildu,
- nieobsłużone wyjątki .NET i Android,
- migrację SQLite,
- zapis i czyszczenie historii oraz ulubionych,
- wybór plików obrazu i audio,
- tworzenie paczki diagnostycznej.

Eksport w aplikacji:

```text
Ustawienia -> Logi i diagnostyka
```

Pobranie awaryjne z buildu Debug:

```powershell
pwsh ./tools/quality/pull-android-diagnostics.ps1 -CreateZip
```

Każdy defekt `FAIL` lub `BLOCKED` musi mieć:

- identyfikator sesji,
- dokładny czas zdarzenia,
- paczkę ZIP albo logcat,
- commit SHA i numer buildu,
- urządzenie oraz wersję Androida,
- screenshot lub nagranie ekranu.

Dokumentacja:

```text
docs/quality/LOCAL_LOGGING.md
```

## 6. UX wyniku

Każdy projekt musi poprawnie współpracować z ekranem wyniku:

- wynik free widoczny bez reklamy,
- wynik premium po mock rewarded ad,
- sekcja `Dlaczego taki wynik?`,
- widoczna reguła i punkty dopasowania,
- widoczne dopasowane odpowiedzi,
- alternatywne rekomendacje, jeśli silnik reguł je zwróci,
- zapisany wynik można ponownie otworzyć z historii i ulubionych.

## 7. Marketing, manual QA i scenariusze użycia

Każdy projekt musi mieć:

```text
projects/<projectId>/marketing/store-listing.pl.md
projects/<projectId>/tests/manual-tests.md
projects/<projectId>/tests/production-scenarios.md
```

`production-scenarios.md` musi zawierać dokładnie pięć scenariuszy `SCN-01`–`SCN-05`. Każdy scenariusz musi mieć:

- cel,
- co najmniej dwa numerowane kroki,
- oczekiwany wynik,
- opis pokrycia.

Manual QA i scenariusze produkcyjne powinny łącznie obejmować:

- wejście z katalogu projektów,
- przejście quizu,
- wynik free,
- odblokowanie premium,
- zapis do ulubionych,
- zapis w historii,
- restart procesu i odtworzenie danych,
- zmianę języka PL/EN/UK,
- przypadek default/fallback,
- bezpieczeństwo domenowe,
- Local AI image/audio dla projektów, które mają je włączone.

Automatyczne gate'y:

```text
tests/AppFactory.Mobile.Tests/ProjectProductionScenariosTests.cs
tests/AppFactory.Mobile.Tests/ScenarioImplementationAuditTests.cs
tests/AppFactory.Mobile.Tests/AllProjectRuleReachabilityTests.cs
tests/AppFactory.Mobile.Tests/LocalLogStoreTests.cs
tests/AppFactory.Mobile.Tests/DiagnosticsProductionTests.cs
```

## 8. Bezpieczeństwo domenowe

Dla projektów technicznych i domowych wymagane są bezpieczne fallbacki:

- brak instrukcji pracy przy prądzie, gazie lub wodzie pod ryzykiem bez fachowca,
- ostrzeżenia przy zalaniu, dymie, zapachu spalenizny, pleśni, niepewnym mocowaniu lub dużym obciążeniu,
- brak zachęty do używania przypadkowej chemii albo mieszania środków.

## 9. Wykonanie testów na Androidzie

Przed wydaniem trzeba wykonać:

- smoke test wspólnego UI,
- test znacznika `LOCAL_TEST_MARKER`,
- eksport ZIP z aplikacji,
- pobranie logów przez ADB dla buildu Debug,
- 100 scenariuszy produkcyjnych,
- test nowej instalacji SQLite,
- test migracji `Preferences -> SQLite`,
- test offline,
- test odmowy uprawnień,
- test FilePicker,
- test systemowego przycisku Wstecz,
- test obrotu ekranu i wznowienia aplikacji,
- test Local AI albo jawne wyłączenie AI w buildzie wydaniowym.

Wyniki zapisuje się w:

```text
docs/quality/SCENARIO_EXECUTION_TRACKER.md
```

## 10. Status produkcyjny projektu

Projekt można oznaczyć jako produkcyjny dopiero gdy:

- przechodzi wszystkie testy automatyczne,
- Android Debug i Release build przechodzą,
- ma komplet danych source i runtime,
- ma `reason` dla każdej reguły,
- ma PL/EN/UK parytet wyników,
- ma manual QA,
- ma dokładnie pięć kompletnych scenariuszy produkcyjnych,
- wszystkie jego scenariusze mają status `PASS`,
- baza SQLite i migracja zostały potwierdzone,
- eksport logów i zbieranie logcat zostały potwierdzone,
- ma listing marketingowy,
- nie ma otwartych defektów krytycznych ani wysokich,
- nie ma znanych blokujących ryzyk w `QUALITY_STATUS.md`.

## 11. Następne rozszerzenia

Po uzyskaniu stabilnego statusu jakości można przygotować:

- testy UI automatyzowane na emulatorze,
- screenshot tests dla ekranów wyników,
- backup/eksport danych użytkownika,
- osobne store listingi EN/UK,
- opcjonalną telemetrykę opt-in z zachowaniem prywatności.
