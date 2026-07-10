# Pokrycie scenariuszy produkcyjnych

## Podsumowanie

- Projekty w katalogu: **20**
- Scenariusze na projekt: **5**
- Łączna liczba zdefiniowanych scenariuszy: **100**
- Automatyczny gate: `ProjectProductionScenariosTests.cs`

Scenariusze są zapisane w:

```text
projects/<projectId>/tests/production-scenarios.md
```

## Pokrycie projektów

| Projekt | SCN-01 | SCN-02 | SCN-03 | SCN-04 | SCN-05 | Łącznie |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| `plama-ratownik` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `kolek-dobieracz` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `pies-trener-7dni` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `bajka-z-rysunku` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `vinted-olx-opis` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `kot-bawi-sie` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `barber-translator` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `outfit-coach` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `domfix` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `fryzury-proste` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `rysunek-coach` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `bukietownik` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `pokoj-makeover` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `pakowanie-paczek` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `silikon-fuga-fix` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `szydelko-pomocnik` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `chleb-zakwas-coach` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `zmywarka-diagnosta` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `krawat-garnitur-coach` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |
| `router-wifi-diagnosta` | ✅ | ✅ | ✅ | ✅ | ✅ | 5 |

## Standard scenariusza

Każdy scenariusz zawiera:

- `Cel`,
- numerowane `Kroki`,
- `Oczekiwany wynik`,
- `Pokrycie`.

## Zakres przekrojowy

Zestaw 100 scenariuszy obejmuje:

- podstawowe dopasowanie reguły,
- wariant alternatywny,
- fallback lub bezpieczeństwo,
- premium, alternatywy, historię albo ulubione,
- PL/EN/UK,
- Local AI image dla 8 projektów,
- Local AI audio dla 3 projektów,
- pobieranie i weryfikację modeli ONNX.

## Ważne rozróżnienie

Status ✅ oznacza, że scenariusz jest **zdefiniowany i objęty automatyczną walidacją struktury**. Nie oznacza jeszcze, że został ręcznie wykonany na fizycznym urządzeniu Android.

Do finalnego statusu produkcyjnego potrzebne są:

1. zielony workflow `Quality Checks`,
2. wykonanie scenariuszy na urządzeniu lub emulatorze,
3. zapis wyniku PASS/FAIL,
4. naprawa wszystkich błędów blokujących.
