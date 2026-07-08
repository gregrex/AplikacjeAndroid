# Zmywarka Diagnosta — raport doprowadzenia do MVP

## Status

Projekt `zmywarka-diagnosta` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/zmywarka-diagnosta/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- zmywarki, która nie domywa,
- osadu na naczyniach,
- brzydkiego zapachu,
- nierozpuszczonej tabletki,
- mokrych naczyń,
- braku odpompowania wody,
- kodu błędu,
- wycieku,
- zapachu spalenizny,
- ryzyka porażenia.

### Zasady bezpieczeństwa

Projekt nie prowadzi użytkownika przez naprawy elektryczne, pompę, moduły sterujące ani rozbieranie urządzenia.

Scenariusze wysokiego priorytetu:

- `safety = leak`,
- `safety = burning_smell`,
- `safety = electric_risk`.

W tych przypadkach aplikacja kieruje użytkownika do przerwania używania urządzenia i kontaktu z serwisem.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/zmywarka-diagnosta`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `ZmywarkaDiagnostaDataTests.cs` — walidacja danych i scenariusze reguł,
- `ZmywarkaDiagnostaLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/zmywarka-diagnosta/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/zmywarka-diagnosta/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Zaktualizowano theme projektu:

- `projects/zmywarka-diagnosta/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/zmywarka-diagnosta/theme.json`.

Motyw: `dishwasher-clean-safe`, czysty i spokojny styl checklist diagnostycznych.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.
- Scenariusze bezpieczeństwa: stop i serwis.

## Uwagi

Aplikacja pomaga w bezpiecznej checklistcie użytkownika. Nie zastępuje serwisu AGD i nie prowadzi przez naprawy elektryczne, pompę, moduły sterujące ani rozbieranie zmywarki.
