# DomFix — raport doprowadzenia do MVP

## Status

Projekt `domfix` został doprowadzony do poziomu MVP zgodnego z aktualnym wzorcem repozytorium `AplikacjeAndroid`.

## Zakres wykonania

### Dane projektowe

Dodano komplet danych w `projects/domfix/data`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`.

Dane obejmują scenariusze dla:

- skrzypiących drzwi,
- luźnego zawiasu,
- drobnej rysy na meblu,
- podstawowego zatkanego odpływu,
- wymiany silikonu,
- odświeżenia fugi,
- zaciętego zamka błyskawicznego,
- drobnego uszczelnienia,
- sytuacji wysokiego ryzyka wymagającej przerwania pracy.

### Bezpieczeństwo

Dodano regułę `high_risk_stop` o najwyższym priorytecie. Jeżeli użytkownik wybierze `risk = high`, aplikacja nie prowadzi przez naprawę, tylko kieruje do zabezpieczenia miejsca i wezwania fachowca.

Projekt nie prowadzi użytkownika przez prace elektryczne, gazowe, konstrukcyjne ani hydrauliczne wysokiego ryzyka.

### Runtime

Dodano mirror danych do `src/AppFactory.Mobile/wwwroot/projects/domfix`, aby aplikacja mobilna mogła ładować projekt przez istniejący `ProjectDataService`.

### Testy

Dodano testy automatyczne:

- `DomFixDataTests.cs` — walidacja danych i scenariusze reguł,
- `DomFixLanguageParityTests.cs` — parytet `resultId` dla PL/EN/UK i kompletność treści.

Dodano testy manualne:

- `projects/domfix/tests/manual-tests.md`.

### Marketing

Dodano listing sklepu:

- `projects/domfix/marketing/store-listing.pl.md`.

### Katalog aplikacji

Dodano wpis do `ProjectCatalogService`, dzięki czemu projekt pojawia się w katalogu aplikacji.

### Theme

Zaktualizowano theme projektu:

- `projects/domfix/theme.json`.

Dodano runtime theme:

- `src/AppFactory.Mobile/wwwroot/projects/domfix/theme.json`.

Motyw: `domfix-teal-safe`, jasny praktyczny styl dla checklist napraw i ostrzeżeń bezpieczeństwa.

## Definicja gotowości MVP

- Dane: gotowe.
- Runtime: gotowy.
- Testy: dodane.
- Marketing: dodany.
- Katalog: dodany.
- Theme: gotowy.
- Raport: dodany.

## Uwagi

Aplikacja pomaga tylko przy drobnych, bezpiecznych naprawach domowych. Przy ryzyku wysokim, instalacjach, gazie, prądzie, konstrukcji lub aktywnym wycieku kieruje użytkownika do przerwania pracy i wezwania fachowca.
