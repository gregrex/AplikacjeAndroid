# Rysunek Coach — raport doprowadzenia do MVP

## Status

Projekt `rysunek-coach` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/rysunek-coach/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- prostego zwierzątka,
- neutralnego pojazdu,
- rośliny w doniczce,
- prostej neutralnej postaci,
- robota z figur geometrycznych,
- małego domku,
- autorskiego potworka,
- uniwersalnej karty ćwiczeń.

### Zasady treści i praw autorskich

Projekt nie używa nazw chronionych postaci, bohaterów z bajek, gier, filmów, marek, logo ani konkretnych modeli.

Tematy są neutralne i autorskie. Celem jest nauka rysowania przez proste figury i kroki, a nie kopiowanie cudzych projektów.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/rysunek-coach`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `RysunekCoachDataTests.cs` — walidacja danych i scenariusze reguł,
- `RysunekCoachLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/rysunek-coach/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/rysunek-coach/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Zaktualizowano theme projektu:

- `projects/rysunek-coach/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/rysunek-coach/theme.json`.

Motyw: `drawing-coach-blue-paper`, jasny styl lekcji rysowania i kart ćwiczeń.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.

## Uwagi

Aplikacja pokazuje proste lekcje rysowania krok po kroku. Nie należy używać jej do kopiowania chronionych postaci, logo, marek ani cudzych projektów.
