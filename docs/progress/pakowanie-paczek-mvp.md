# Pakowanie Paczek — raport doprowadzenia do MVP

## Status

Projekt `pakowanie-paczek` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/pakowanie-paczek/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- ubrań,
- książek,
- szkła,
- elektroniki,
- kosmetyków,
- butów,
- małych elementów zestawów,
- małego sprzętu,
- uniwersalnej checklisty paczki.

### Zasady treści

Projekt nie gwarantuje bezpieczeństwa przesyłki. Przy wartościowych, bardzo delikatnych albo nietypowych rzeczach wyniki przypominają o ubezpieczeniu, zdjęciach stanu przed pakowaniem i zasadach przewoźnika.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/pakowanie-paczek`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

Uwaga techniczna: runtime `rules.json` zapisano w neutralnej wersji, ponieważ connector GitHub blokował pełną wersję przy części nazw. Pełny zestaw reguł znajduje się w danych źródłowych projektu.

### Testy

Dodano testy automatyczne:

- `PakowaniePaczekDataTests.cs` — walidacja danych i scenariusze reguł,
- `PakowaniePaczekLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/pakowanie-paczek/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/pakowanie-paczek/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Projekt korzysta z theme:

- `projects/pakowanie-paczek/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/pakowanie-paczek/theme.json`.

Motyw: `parcel-safe`, praktyczny styl checklisty pakowania.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.

## Uwagi

Aplikacja pomaga dobrać sposób zabezpieczenia paczki, ale nie zastępuje zasad przewoźnika i nie gwarantuje bezpieczeństwa transportu.
