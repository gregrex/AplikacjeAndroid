# Tracker wykonania scenariuszy produkcyjnych

## Zasady

Dozwolone statusy:

- `NOT_RUN` — scenariusz nie został wykonany,
- `PASS` — wynik zgodny z oczekiwaniem,
- `FAIL` — wynik niezgodny; wymagany numer defektu,
- `BLOCKED` — scenariusza nie można wykonać z powodu zależności, konfiguracji lub modelu.

Każdy status `FAIL` lub `BLOCKED` powinien mieć opis w kolumnie Uwagi/defekt.

## Macierz 20 × 5

| Projekt | SCN-01 | SCN-02 | SCN-03 | SCN-04 | SCN-05 | Wynik | Uwagi/defekt |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `plama-ratownik` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `kolek-dobieracz` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `pies-trener-7dni` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `bajka-z-rysunku` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `vinted-olx-opis` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `kot-bawi-sie` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `barber-translator` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `outfit-coach` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `domfix` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `fryzury-proste` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `rysunek-coach` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `bukietownik` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `pokoj-makeover` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `pakowanie-paczek` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `silikon-fuga-fix` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `szydelko-pomocnik` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `chleb-zakwas-coach` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `zmywarka-diagnosta` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `krawat-garnitur-coach` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `router-wifi-diagnosta` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |

## Kryterium wydania

Wersja może otrzymać status `production-ready`, gdy:

- wszystkie testy automatyczne są zielone,
- wszystkie 100 scenariuszy ma status `PASS`,
- nie ma otwartych defektów krytycznych ani wysokich,
- modele Local AI są skonfigurowane i przetestowane albo funkcje AI są wyłączone w buildzie publikacyjnym.
