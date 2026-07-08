# Fryzury Proste — raport doprowadzenia do MVP

## Status

Projekt `fryzury-proste` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/fryzury-proste/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- szybkiego uczesania w kilka minut,
- sportowego upięcia,
- codziennego naturalnego uczesania,
- prostego eleganckiego uczesania,
- delikatnego ułożenia loków,
- uczesania z klamrą,
- domyślnego planu uczesania.

### Zasady treści

Projekt pokazuje neutralne instrukcje techniczne wykonania uczesania. Nie ocenia osoby, twarzy, kształtu twarzy, atrakcyjności ani wyglądu.

Instrukcje unikają bolesnego spinania, szarpania włosów i agresywnego rozczesywania loków.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/fryzury-proste`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `FryzuryProsteDataTests.cs` — walidacja danych i scenariusze reguł,
- `FryzuryProsteLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/fryzury-proste/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/fryzury-proste/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Dodano theme projektu:

- `projects/fryzury-proste/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/fryzury-proste/theme.json`.

Motyw: `hair-simple-violet`, jasny neutralny styl dla instrukcji uczesań krok po kroku.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.

## Uwagi

Aplikacja nie ocenia osoby, twarzy ani atrakcyjności. Pokazuje wyłącznie neutralne instrukcje wykonania prostego uczesania.
