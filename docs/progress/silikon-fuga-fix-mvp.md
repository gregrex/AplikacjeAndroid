# Silikon Fuga Fix — raport doprowadzenia do MVP

## Status

Projekt `silikon-fuga-fix` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/silikon-fuga-fix/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- zabrudzonej fugi,
- starego silikonu,
- pleśni na silikonie,
- pęknięcia,
- odspojenia,
- podejrzenia przecieku,
- wysokiego ryzyka,
- uniwersalnej oceny silikonu lub fugi.

### Zasady bezpieczeństwa

Projekt rozdziela proste prace DIY od sytuacji, w których trzeba przerwać pracę i skontaktować się z fachowcem.

Aplikacja nie prowadzi przez:

- naprawę instalacji wodnej,
- ukrywanie aktywnego przecieku,
- pracę na mokrym albo miękkim podłożu,
- diagnozowanie wilgoci pod płytkami,
- prace konstrukcyjne.

Przy podejrzeniu przecieku albo wysokim ryzyku wyniki kierują do zabezpieczenia miejsca, zebrania informacji i kontaktu z fachowcem.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/silikon-fuga-fix`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `SilikonFugaFixDataTests.cs` — walidacja danych i scenariusze reguł,
- `SilikonFugaFixLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/silikon-fuga-fix/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/silikon-fuga-fix/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Zaktualizowano theme projektu:

- `projects/silikon-fuga-fix/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/silikon-fuga-fix/theme.json`.

Motyw: `sealant-fix-slate-blue`, techniczny i bezpieczny styl dla checklist naprawczych.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.
- Scenariusze ryzykowne: kierują do fachowca.

## Uwagi

Aplikacja pomaga przy prostych pracach domowych związanych z silikonem i fugami. Nie zastępuje fachowej diagnozy przy przecieku, wilgoci pod powierzchnią, miękkim podłożu albo problemie instalacji.
