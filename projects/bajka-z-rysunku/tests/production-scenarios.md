# Scenariusze produkcyjne — Bajka z rysunku

## SCN-01 — Bajka z prostego rysunku zwierzęcia

**Cel:** sprawdzić podstawowe wygenerowanie opowieści.

**Kroki:**
1. Otwórz projekt `bajka-z-rysunku`.
2. Wybierz motyw zwierzęcia i spokojny ton.
3. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje spójną, spokojną bajkę opartą o wybrany motyw.

**Pokrycie:** pytania, reguła szczegółowa, wynik free.

## SCN-02 — Alternatywny bohater i nastrój

**Cel:** sprawdzić różnicowanie treści.

**Kroki:**
1. Wybierz inny motyw niż w SCN-01.
2. Zmień nastrój lub miejsce akcji.
3. Otwórz wynik premium.

**Oczekiwany wynik:** historia różni się bohaterem, tonem i przebiegiem, a pełna wersja zawiera rozwinięcie.

**Pokrycie:** alternatywna reguła, premium, różnicowanie wyników.

## SCN-03 — Brak szczegółowego dopasowania

**Cel:** sprawdzić fallback.

**Kroki:**
1. Wybierz kombinację odpowiedzi bez dedykowanej reguły.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje bezpieczną bajkę domyślną, bez pustej strony i bez błędu nawigacji.

**Pokrycie:** wildcard fallback, wynik domyślny.

## SCN-04 — Zapis bajki

**Cel:** sprawdzić historię i ulubione.

**Kroki:**
1. Wygeneruj pełną bajkę.
2. Dodaj ją do ulubionych.
3. Otwórz historię i ulubione.

**Oczekiwany wynik:** właściwy wynik jest widoczny w obu miejscach z poprawnym projektem.

**Pokrycie:** premium, historia, ulubione.

## SCN-05 — Języki PL/EN/UK

**Cel:** sprawdzić parytet treści.

**Kroki:**
1. Wygeneruj tę samą kombinację w PL, EN i UK.
2. Porównaj identyfikatory i strukturę wyniku.

**Oczekiwany wynik:** `resultId` pozostaje identyczny, tytuł, streszczenie i kroki istnieją w każdym języku lub korzystają z jawnego fallbacku.

**Pokrycie:** lokalizacja, parytet wyników, fallback językowy.
