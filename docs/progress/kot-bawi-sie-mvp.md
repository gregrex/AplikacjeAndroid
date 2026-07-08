# Kot Bawi się — raport doprowadzenia do MVP

## Status

Projekt `kot-bawi-sie` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/kot-bawi-sie/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- kociaka z dużą energią,
- kota seniora,
- kota aktywnego,
- zabaw DIY z kartonem,
- zabaw węchowych z karmą,
- krótkich mikrosesji,
- spokojnych kotów,
- domyślnego kota niewychodzącego.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/kot-bawi-sie`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `KotBawiSieDataTests.cs` — walidacja danych i scenariusze reguł,
- `KotBawiSieLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/kot-bawi-sie/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/kot-bawi-sie/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Dodano motyw:

- `projects/kot-bawi-sie/theme.json`,
- `src/AppFactory.Mobile/wwwroot/projects/kot-bawi-sie/theme.json`.

Motyw: `cat-play-purple`, ciepły fioletowo-pomarańczowy styl dla aplikacji właściciela kota.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: dodany.
- Raport: dodany.

## Uwagi

Aplikacja zawiera jasne ograniczenie: nie diagnozuje zdrowia kota i nie zastępuje konsultacji weterynaryjnej. Scenariusze unikają zachęt do zabawy rękami, wymuszania aktywności oraz używania ostrych lub niebezpiecznych elementów.
