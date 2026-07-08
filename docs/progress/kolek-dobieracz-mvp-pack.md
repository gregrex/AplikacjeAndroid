# Progress — Kołek Dobieracz MVP pack

## Status

Projekt `kolek-dobieracz` dostał pierwszą kompletną paczkę MVP.

## Dodane

- `data/app.json`
- `data/categories.json`
- `data/questions.json`
- `data/rules.json`
- `data/results.pl.json`
- `theme.json`
- `tests/manual-tests.md`
- `marketing/store-listing.pl.md`
- `KolekDobieraczDataTests.cs`

## Zakres MVP

Projekt obsługuje:

- beton,
- cegłę,
- karton-gips,
- drewno,
- lekki ciężar,
- średni ciężar,
- ciężki lub ryzykowny montaż,
- fallback ogólny.

## Testy automatyczne

Dodany test sprawdza:

1. integralność danych przez wspólny walidator,
2. scenariusz beton + lekki przedmiot,
3. scenariusz ciężki montaż i ostrzeżenie ryzyka.

## Ocena ukończenia

`kolek-dobieracz`: około 45%.

## Nadal brakuje

- wyników EN i UK,
- wpięcia projektu do runtime app selector,
- testu językowego,
- builda Android,
- trwałego storage,
- prawdziwego AdMob lub adaptera,
- sprawdzenia UI na telefonie.
