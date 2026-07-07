# Status kompletności 20 projektów

## Cel

Ten plik pokazuje stan przygotowania katalogów projektowych w `AplikacjeAndroid`.

## Status

| # | Projekt | README | Prompt Codex | Dane startowe | Priorytet |
|---:|---|---|---|---|---:|
| 1 | plama-ratownik | tak | tak | częściowo gotowe | 1 |
| 2 | kolek-dobieracz | tak | tak | do przygotowania | 2 |
| 3 | pies-trener-7dni | tak | tak | do przygotowania | 3 |
| 4 | bajka-z-rysunku | tak | tak | do przygotowania | 4 |
| 5 | vinted-olx-opis | tak | tak | do przygotowania | 5 |
| 6 | kot-bawi-sie | tak | tak | do przygotowania | 6 |
| 7 | barber-translator | tak | tak | do przygotowania | 7 |
| 8 | outfit-coach | tak | tak | do przygotowania | 8 |
| 9 | pakowanie-paczek | tak | tak | do przygotowania | 9 |
| 10 | pokoj-makeover | tak | tak | do przygotowania | 10 |
| 11 | domfix | tak | tak | do przygotowania | 11 |
| 12 | zmywarka-diagnosta | tak | tak | do przygotowania | 12 |
| 13 | router-wifi-diagnosta | tak | tak | do przygotowania | 13 |
| 14 | szydelko-pomocnik | tak | tak | do przygotowania | 14 |
| 15 | rysunek-coach | tak | tak | do przygotowania | 15 |
| 16 | bukietownik | tak | tak | do przygotowania | 16 |
| 17 | chleb-zakwas-coach | tak | tak | do przygotowania | 17 |
| 18 | silikon-fuga-fix | tak | tak | do przygotowania | 18 |
| 19 | krawat-garnitur-coach | tak | tak | do przygotowania | 19 |
| 20 | fryzury-proste | tak | tak | do przygotowania | 20 |

## Najbliższy kierunek pracy

1. Najpierw dokończyć kod wspólnego silnika MAUI Blazor Hybrid.
2. Następnie dopiąć pełne dane JSON dla `plama-ratownik`.
3. Potem tworzyć paczki danych dla projektów 2–5.
4. Dopiero po pierwszym działającym buildzie rozszerzać projekty 6–20.

## Minimalna paczka danych dla każdego projektu

Każdy projekt powinien docelowo mieć:

- `data/app.json`
- `data/categories.json`
- `data/questions.json`
- `data/rules.json`
- `data/results.pl.json`
- `data/i18n/pl.md`
- `marketing/store-listing.pl.md`
- `tests/manual-tests.md`

## Uwaga

Na tym etapie projekty są przygotowane organizacyjnie. Tylko `plama-ratownik` ma już częściową paczkę danych gotową do podłączenia pod silnik.
