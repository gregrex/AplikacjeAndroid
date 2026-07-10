# Checklist produkcyjny AppFactory

Ten dokument opisuje minimalne warunki przejścia projektu z MVP do statusu produkcyjnego.

## 1. Dane projektu

Każdy projekt musi mieć komplet źródeł:

```text
projects/<projectId>/data/app.json
projects/<projectId>/data/categories.json
projects/<projectId>/data/questions.json
projects/<projectId>/data/rules.json
projects/<projectId>/data/results.pl.json
projects/<projectId>/data/results.en.json
projects/<projectId>/data/results.uk.json
projects/<projectId>/theme.json
```

Wymagania:

- `appId` zgodny z katalogiem projektu,
- unikalne ID kategorii, pytań, reguł i wyników,
- każda reguła wskazuje istniejący wynik free i premium,
- każda reguła ma pole `reason`,
- PL/EN/UK mają ten sam zestaw `resultId`,
- każdy wynik ma `title`, `summary` i przynajmniej jeden krok.

## 2. Runtime mirror

Każdy projekt musi mieć komplet runtime:

```text
src/AppFactory.Mobile/wwwroot/projects/<projectId>/app.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/categories.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/questions.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/rules.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/results.pl.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/results.en.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/results.uk.json
src/AppFactory.Mobile/wwwroot/projects/<projectId>/theme.json
```

Runtime powinien być synchronizowany z `projects` poleceniem:

```powershell
pwsh ./tools/quality/sync-runtime-packs.ps1
```

## 3. Testy jakości

Przed statusem produkcyjnym uruchom:

```powershell
pwsh ./tools/quality/run-quality-checks.ps1 -SyncRuntimeFirst -WriteReport
```

Testy muszą przejść bez błędów.

Workflow GitHub Actions:

```text
.github/workflows/quality-checks.yml
```

uruchamia testy jakości na push, pull request i ręczne wywołanie.

## 4. UX wyniku

Każdy projekt musi poprawnie współpracować z ekranem wyniku:

- wynik free widoczny bez reklamy,
- wynik premium po mock rewarded ad,
- sekcja `Dlaczego taki wynik?`,
- widoczna reguła i punkty dopasowania,
- widoczne dopasowane odpowiedzi,
- alternatywne rekomendacje, jeśli silnik reguł je zwróci.

## 5. Marketing, manual QA i scenariusze użycia

Każdy projekt musi mieć:

```text
projects/<projectId>/marketing/store-listing.pl.md
projects/<projectId>/tests/manual-tests.md
projects/<projectId>/tests/production-scenarios.md
```

`production-scenarios.md` musi zawierać dokładnie pięć scenariuszy `SCN-01`–`SCN-05`. Każdy scenariusz musi mieć:

- cel,
- co najmniej dwa numerowane kroki,
- oczekiwany wynik,
- opis pokrycia.

Manual QA i scenariusze produkcyjne powinny łącznie obejmować:

- wejście z katalogu projektów,
- przejście quizu,
- wynik free,
- odblokowanie premium,
- zapis do ulubionych,
- zapis w historii,
- zmianę języka PL/EN/UK,
- przypadek default/fallback,
- bezpieczeństwo domenowe,
- Local AI image/audio dla projektów, które mają je włączone.

Automatyczny gate:

```text
tests/AppFactory.Mobile.Tests/ProjectProductionScenariosTests.cs
```

sprawdza komplet pięciu scenariuszy oraz pokrycie ONNX dla projektów obrazu i dźwięku.

## 6. Bezpieczeństwo domenowe

Dla projektów technicznych i domowych wymagane są bezpieczne fallbacki:

- brak instrukcji pracy przy prądzie, gazie lub wodzie pod ryzykiem bez fachowca,
- ostrzeżenia przy zalaniu, dymie, zapachu spalenizny, pleśni, niepewnym mocowaniu lub dużym obciążeniu,
- brak zachęty do używania przypadkowej chemii albo mieszania środków.

## 7. Status produkcyjny projektu

Projekt można oznaczyć jako produkcyjny dopiero gdy:

- przechodzi testy jakości,
- ma komplet danych source i runtime,
- ma `reason` dla każdej reguły,
- ma PL/EN/UK parytet wyników,
- ma manual QA,
- ma dokładnie pięć kompletnych scenariuszy produkcyjnych,
- ma listing marketingowy,
- nie ma znanych blokujących ryzyk w `QUALITY_STATUS.md`.

## 8. Następne rozszerzenia

Po uzyskaniu stabilnego statusu jakości można przygotować:

- osobne build profile per projekt,
- osobne nazwy aplikacji i ikony,
- osobne store listingi EN/UK,
- automatyczny raport pokrycia projektów,
- testy UI/snapshoty dla ekranu wyniku.
