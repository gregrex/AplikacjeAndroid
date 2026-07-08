# Chleb Zakwas Coach — raport doprowadzenia do MVP

## Status

Projekt `chleb-zakwas-coach` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/chleb-zakwas-coach/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- zakwasu, który nie rośnie,
- ciasta, które nie wyrasta,
- zakalca albo zbitego miękiszu,
- bladej skórki,
- pękania chleba,
- podejrzanego zapachu,
- pleśni,
- harmonogramu karmienia,
- uniwersalnej korekty wypieku.

### Zasady bezpieczeństwa żywności

Projekt ma reguły wysokiego priorytetu dla:

- `smell = mold`,
- `smell = rotten`.

W tych scenariuszach aplikacja zaleca wyrzucenie zakwasu i rozpoczęcie od nowa. Nie prowadzi użytkownika przez ratowanie spleśniałego albo podejrzanie pachnącego zakwasu.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/chleb-zakwas-coach`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `ChlebZakwasCoachDataTests.cs` — walidacja danych i scenariusze reguł,
- `ChlebZakwasCoachLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/chleb-zakwas-coach/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/chleb-zakwas-coach/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Zaktualizowano theme projektu:

- `projects/chleb-zakwas-coach/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/chleb-zakwas-coach/theme.json`.

Motyw: `sourdough-warm-bread`, ciepły styl dla domowego pieczenia chleba.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.
- Pleśń i zapach zepsucia: prowadzą do wyrzucenia zakwasu.

## Uwagi

Aplikacja pomaga w domowym prowadzeniu zakwasu i korekcie podstawowych problemów z chlebem. Nie zastępuje pełnej oceny bezpieczeństwa żywności. Przy pleśni, podejrzanym nalocie albo zapachu zepsucia zakwas należy wyrzucić.
