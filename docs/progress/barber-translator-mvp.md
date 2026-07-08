# Barber Translator — raport doprowadzenia do MVP

## Status

Projekt `barber-translator` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/barber-translator/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- krótkiego fade,
- klasycznego stylu formalnego,
- naturalnej średniej długości,
- kręconych włosów,
- teksturowanej grzywki,
- soft taper,
- zachowania długości,
- domyślnego briefu fryzury.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/barber-translator`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `BarberTranslatorDataTests.cs` — walidacja danych i scenariusze reguł,
- `BarberTranslatorLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/barber-translator/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/barber-translator/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Projekt miał już motyw:

- `projects/barber-translator/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/barber-translator/theme.json`.

Motyw: `barber-dark`, ciemny kontrastowy styl premium dla instrukcji do pokazania barberowi.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.

## Uwagi

Aplikacja nie ocenia wyglądu osoby i nie obiecuje konkretnego efektu wizualnego. Jej zadaniem jest uporządkowanie preferencji użytkownika w krótką, czytelną instrukcję dla fryzjera lub barbera.
