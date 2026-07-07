# Progress — Plama Ratownik language pack

## Zrobione

Dodano wyniki w trzech językach:

- `results.pl.json`
- `results.en.json`
- `results.uk.json`

Dodano testy parytetu językowego:

- `PlamaRatownikLanguageParityTests.cs`

## Co test sprawdza

1. Czy PL, EN i UK mają ten sam zestaw `id` wyników.
2. Czy każdy wynik ma tytuł.
3. Czy każdy wynik ma podsumowanie.
4. Czy każdy wynik ma kroki.

## Znaczenie

To jest pierwszy krok do obsługi wielu języków w aplikacjach.

## Status projektu

`plama-ratownik`: około 60%.

## Nadal brakuje

- zielone testy CI,
- zielony Android build,
- wybór języka w UI podłączony do wyboru pliku wyników,
- tłumaczenia etykiet kategorii i pytań w runtime,
- storage trwały,
- AdMob albo adapter produkcyjny,
- checklista Google Play.
