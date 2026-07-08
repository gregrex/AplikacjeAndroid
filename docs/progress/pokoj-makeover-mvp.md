# Pokój Makeover — raport doprowadzenia do MVP

## Status

Projekt `pokoj-makeover` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/pokoj-makeover/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- stylu minimalistycznego,
- pokoju przytulnego,
- pokoju kolorowego,
- strefy gamingowej,
- nauki i pracy,
- małego pokoju,
- metamorfozy za 0 zł,
- uniwersalnego planu makeover.

### Zasady MVP

Projekt nie analizuje zdjęć w MVP. Użytkownik opisuje pokój przez formularz: typ pokoju, styl, budżet, problem i czas.

Projekt skupia się na zmianach bez remontu: porządku, świetle, ustawieniu rzeczy, dekoracjach DIY, przechowywaniu i prostych zakupach.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/pokoj-makeover`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `PokojMakeoverDataTests.cs` — walidacja danych i scenariusze reguł,
- `PokojMakeoverLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/pokoj-makeover/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/pokoj-makeover/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Zaktualizowano theme projektu:

- `projects/pokoj-makeover/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/pokoj-makeover/theme.json`.

Motyw: `room-makeover-indigo-warm`, jasny praktyczny styl dla planów metamorfozy pokoju.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.

## Uwagi

Aplikacja pomaga zaplanować prostą zmianę pokoju bez remontu i bez dużego budżetu. Nie analizuje zdjęć w MVP i nie zastępuje projektanta wnętrz.
