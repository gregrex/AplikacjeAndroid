# Scenariusze produkcyjne — Szydełko Pomocnik

## SCN-01 — Licznik rzędów

**Cel:** sprawdzić podstawową funkcję użytkową.

**Kroki:**
1. Otwórz projekt `szydelko-pomocnik`.
2. Rozpocznij nowy projekt robótki.
3. Zwiększ licznik o kilka rzędów.
4. Zamknij i ponownie otwórz aplikację.

**Oczekiwany wynik:** licznik zachowuje ostatnią wartość i jest przypisany do właściwego projektu.

**Pokrycie:** stan lokalny, trwałość, restart.

## SCN-02 — Prosty wzór dla początkującego

**Cel:** sprawdzić podstawową rekomendację wzoru.

**Kroki:**
1. Wybierz poziom początkujący.
2. Wybierz prosty typ projektu.
3. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje zrozumiały wzór krok po kroku i wyjaśnia poziom trudności.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-03 — Błąd w robótce

**Cel:** sprawdzić scenariusz naprawczy.

**Kroki:**
1. Wybierz problem z pominiętym oczkiem lub nierównym rzędem.
2. Otwórz wynik premium.

**Oczekiwany wynik:** wynik podaje bezpieczny sposób cofnięcia lub poprawy robótki i nie wymaga rozpoczynania całości od nowa bez potrzeby.

**Pokrycie:** alternatywna reguła, premium, korekta błędu.

## SCN-04 — Notatki projektu

**Cel:** sprawdzić zapis notatek.

**Kroki:**
1. Dodaj notatkę o włóczce, szydełku i zmianie wzoru.
2. Zamknij aplikację.
3. Otwórz projekt ponownie.

**Oczekiwany wynik:** notatka pozostaje dostępna i nie miesza się z innym projektem.

**Pokrycie:** lokalny zapis, separacja danych.

## SCN-05 — Fallback i języki

**Cel:** sprawdzić kompletność wyników.

**Kroki:**
1. Wybierz nietypową kombinację odpowiedzi.
2. Otwórz wynik w PL, EN i UK.

**Oczekiwany wynik:** aplikacja pokazuje bazową pomoc, bez pustych kroków, a `resultId` pozostaje spójny między językami.

**Pokrycie:** fallback, PL/EN/UK, parytet wyników.
