# Krawat Garnitur Coach — raport doprowadzenia do MVP

## Status

Projekt `krawat-garnitur-coach` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/krawat-garnitur-coach/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- rozmowy,
- spotkania biznesowego,
- egzaminu,
- uroczystości rodzinnej,
- eleganckiego wyjścia,
- czarnego garnituru,
- zestawu bez krawata,
- uniwersalnej checklisty formalnej.

### Zasady treści

Projekt nie ocenia osoby, twarzy, sylwetki ani atrakcyjności. Wyniki oceniają wyłącznie zgodność elementów stroju z okazją.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/krawat-garnitur-coach`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `KrawatGarniturCoachDataTests.cs` — walidacja danych i scenariusze reguł,
- `KrawatGarniturCoachLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/krawat-garnitur-coach/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/krawat-garnitur-coach/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Zaktualizowano theme projektu:

- `projects/krawat-garnitur-coach/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/krawat-garnitur-coach/theme.json`.

Motyw: `formal-classic-coach`, klasyczny styl dla formalnych checklist stroju.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.
- Zasada nieoceniania osoby: zachowana.

## Uwagi

Aplikacja pomaga dobrać formalny zestaw do okazji. Nie zastępuje stylisty i nie ocenia wyglądu użytkownika.
