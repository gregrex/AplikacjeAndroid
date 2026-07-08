# Szydełko Pomocnik — raport doprowadzenia do MVP

## Status

Projekt `szydelko-pomocnik` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/szydelko-pomocnik/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- licznika rzędów,
- notatek projektu,
- słownika skrótów,
- prostych ściegów,
- szalika,
- czapki,
- podkładki,
- amigurumi,
- koca,
- uniwersalnej karty projektu.

### Zasady MVP

Projekt działa offline i nie wymaga AI. Dane są statyczne, a wynik opiera się na formularzu i regułach.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/szydelko-pomocnik`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `SzydelkoPomocnikDataTests.cs` — walidacja danych i scenariusze reguł,
- `SzydelkoPomocnikLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/szydelko-pomocnik/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/szydelko-pomocnik/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Zaktualizowano theme projektu:

- `projects/szydelko-pomocnik/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/szydelko-pomocnik/theme.json`.

Motyw: `crochet-soft-craft`, spokojny styl dla projektów rękodzielniczych.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.
- MVP offline bez AI: zachowane.

## Uwagi

Aplikacja pomaga prowadzić proste projekty szydełkowe i organizować notatki, licznik rzędów, materiały oraz podstawowe wzory. Nie zastępuje szczegółowego kursu szydełkowania ani indywidualnej korekty techniki.
