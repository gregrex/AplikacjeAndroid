# Bukietownik — raport doprowadzenia do MVP

## Status

Projekt `bukietownik` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/bukietownik/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- bukietu urodzinowego,
- bukietu z podziękowaniem,
- bukietu rodzinnego na stół,
- dekoracji domu,
- bukietu bez okazji,
- róż w romantycznym stylu,
- uniwersalnego bukietu mieszanego.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/bukietownik`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `BukietownikDataTests.cs` — walidacja danych i scenariusze reguł,
- `BukietownikLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/bukietownik/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/bukietownik/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Zaktualizowano theme projektu:

- `projects/bukietownik/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/bukietownik/theme.json`.

Motyw: `bouquet-pink-green`, jasny, świeży styl dla kompozycji kwiatowych.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.

## Uwagi

Aplikacja pomaga osobom początkującym dobrać prostą kompozycję kwiatową do okazji, kolorów, dostępnych kwiatów i stylu. Nie zastępuje profesjonalnego florysty przy kompozycjach ślubnych, pogrzebowych, bardzo formalnych lub drogich.
