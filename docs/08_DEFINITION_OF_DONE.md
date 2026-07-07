# Definition of Done — AplikacjeAndroid

## Cel

Ten dokument określa, kiedy projekt aplikacji w repo `AplikacjeAndroid` można uznać za skończony.

## Status na teraz

Na ten moment żaden projekt nie jest ukończony produkcyjnie.

Najbliżej ukończenia jest:

```text
plama-ratownik
```

Powód: ma częściowe dane, szkielet MAUI Blazor Hybrid, silnik reguł, mock reklam oraz testy jednostkowo-integracyjne.

## Kiedy projekt uznajemy za skończony

Projekt można oznaczyć jako skończony dopiero wtedy, gdy spełnia wszystkie warunki:

1. Aplikacja buduje się komendą Android build.
2. Testy jednostkowe i integracyjne przechodzą.
3. Projekt ma pełną paczkę danych.
4. Projekt ma komplet minimalnych tłumaczeń PL/EN/UK albo świadomy fallback.
5. Aplikacja przechodzi pełny scenariusz użytkownika.
6. Historia i ulubione działają lokalnie.
7. Reklamy rewarded są przynajmniej zamockowane i mają miejsce na integrację AdMob.
8. Istnieją testy integralności danych.
9. Istnieją testy scenariuszy biznesowych.
10. Jest instrukcja uruchomienia.
11. Jest marketing/store listing.
12. Jest lista ryzyk i ostrzeżeń dla użytkownika.
13. Jest checklista publikacji Google Play.

## Status projektów

| Projekt | Status | Ocena ukończenia |
|---|---|---:|
| plama-ratownik | MVP w budowie, najbliżej ukończenia | 45% |
| kolek-dobieracz | opis i prompt, brak pełnych danych | 10% |
| pies-trener-7dni | opis i prompt, brak pełnych danych | 10% |
| bajka-z-rysunku | opis i prompt, brak pełnych danych | 10% |
| vinted-olx-opis | opis i prompt, brak pełnych danych | 10% |
| pozostałe projekty | katalogi i prompty, brak danych | 5-10% |

## Najbliższa ścieżka do pierwszego ukończonego projektu

Aby `plama-ratownik` był pierwszym skończonym projektem, trzeba wykonać:

1. Uruchomić `dotnet test`.
2. Uruchomić Android build.
3. Poprawić błędy kompilacji.
4. Rozbudować dane do minimum 30 reguł.
5. Dodać test unikalności ID.
6. Dodać test kompletności wyników.
7. Dodać SQLite albo świadomie zostawić storage in-memory tylko jako MVP dev.
8. Dodać ekran checklisty Google Play.
9. Dodać EN/UK albo fallback.
10. Przygotować pierwszą wersję release candidate.

## Decyzja

Nie oznaczamy żadnego projektu jako skończonego, dopóki build i testy nie przejdą lokalnie albo w CI.
