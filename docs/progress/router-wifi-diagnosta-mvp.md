# Router WiFi Diagnosta — raport doprowadzenia do MVP

## Status

Projekt `router-wifi-diagnosta` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/router-wifi-diagnosta/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- słabego zasięgu,
- niskiej prędkości,
- zrywania połączenia,
- problemu w jednym pokoju,
- wielu urządzeń,
- złego miejsca routera,
- wielu ścian,
- uniwersalnej diagnozy WiFi.

### Zasady działania

Projekt nie zmienia ustawień routera automatycznie, nie loguje się do panelu routera i nie wykonuje konfiguracji. Wyniki są checklistami oraz instrukcjami do samodzielnego, spokojnego sprawdzenia.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/router-wifi-diagnosta`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `RouterWifiDiagnostaDataTests.cs` — walidacja danych i scenariusze reguł,
- `RouterWifiDiagnostaLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/router-wifi-diagnosta/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/router-wifi-diagnosta/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Dodano theme projektu:

- `projects/router-wifi-diagnosta/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/router-wifi-diagnosta/theme.json`.

Motyw: `wifi-blue-diagnostic`, techniczny i spokojny styl dla checklist domowego WiFi.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.
- Brak automatycznej konfiguracji routera: zachowane.

## Uwagi

Aplikacja pomaga poprawić domowe WiFi przez testy i checklisty. Nie zastępuje pomocy technicznej operatora, nie loguje się do routera i nie zmienia ustawień automatycznie.
