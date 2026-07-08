# Outfit Coach — raport doprowadzenia do MVP

## Status

Projekt `outfit-coach` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/outfit-coach/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- szkoły lub uczelni,
- spotkania smart casual,
- rozmowy lub rekrutacji,
- imprezy lub wyjścia,
- spaceru i pogody,
- uroczystości rodzinnej,
- deszczu,
- chłodu i warstw,
- domyślnego planu stroju.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/outfit-coach`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `OutfitCoachDataTests.cs` — walidacja danych i scenariusze reguł,
- `OutfitCoachLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/outfit-coach/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/outfit-coach/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Dodano theme projektu:

- `projects/outfit-coach/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/outfit-coach/theme.json`.

Motyw: `outfit-soft-blue`, jasny praktyczny styl dla checklisty stroju.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.

## Uwagi

Aplikacja ocenia wyłącznie dopasowanie ubrań do okazji, pogody i wygody. Nie ocenia ciała, wagi, atrakcyjności ani osoby.
