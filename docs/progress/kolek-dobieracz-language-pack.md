# Progress — Kolek Dobieracz language pack

## Zrobione

Dodano wyniki w trzech jezykach:

- `results.pl.json`
- `results.en.json`
- `results.uk.json`

Dodano test zgodnosci jezykow:

- `KolekDobieraczLanguageParityTests.cs`

## Co sprawdza test

1. Czy PL, EN i UK maja ten sam zestaw identyfikatorow wynikow.
2. Czy kazdy wynik ma tytul.
3. Czy kazdy wynik ma podsumowanie.
4. Czy kazdy wynik ma kroki.

## Status projektu

`kolek-dobieracz`: okolo 52%.

## Nadal brakuje

- wpiecia do wyboru projektu w aplikacji,
- skopiowania danych do runtime package,
- testu runtime selector,
- builda Android,
- testu manualnego na telefonie.
