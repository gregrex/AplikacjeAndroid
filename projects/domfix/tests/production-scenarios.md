# Scenariusze produkcyjne — DomFix

## SCN-01 — Luźna klamka

**Cel:** sprawdzić podstawową drobną naprawę.

**Kroki:**
1. Otwórz projekt `domfix`.
2. Wybierz problem z luźną klamką.
3. Zaznacz brak oznak uszkodzenia elektrycznego lub wodnego.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje prostą checklistę naprawy i uzasadnia dobór procedury.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Drobny wyciek

**Cel:** sprawdzić wariant wymagający ostrożności.

**Kroki:**
1. Wybierz problem z wodą.
2. Zaznacz niewielki, kontrolowany wyciek.
3. Otwórz wynik premium.

**Oczekiwany wynik:** wynik zaczyna się od zabezpieczenia miejsca i odcięcia źródła wody, a pełna instrukcja nie przekracza bezpiecznego zakresu DIY.

**Pokrycie:** safety, premium, kolejność kroków.

## SCN-03 — Prąd, gaz lub poważne uszkodzenie

**Cel:** sprawdzić blokadę ryzykownej instrukcji.

**Kroki:**
1. Wybierz odpowiedzi wskazujące prąd, gaz, dym albo zalanie.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja nie podaje instrukcji naprawy, tylko bezpieczny fallback i zalecenie kontaktu z fachowcem.

**Pokrycie:** safety fallback, ograniczenie domenowe.

## SCN-04 — Wynik alternatywny i zapis

**Cel:** sprawdzić alternatywy oraz trwałość.

**Kroki:**
1. Wygeneruj wynik z kilkoma możliwymi przyczynami.
2. Odblokuj premium.
3. Przełącz alternatywę.
4. Dodaj do ulubionych i sprawdź historię.

**Oczekiwany wynik:** aktywna alternatywa jest zapisana z właściwym `ResultId`, a historia zawiera wpis projektu.

**Pokrycie:** alternatywy, premium, ulubione, historia.

## SCN-05 — Języki i fallback

**Cel:** sprawdzić kompletność danych.

**Kroki:**
1. Wybierz nietypową kombinację odpowiedzi.
2. Otwórz wynik w PL, EN i UK.

**Oczekiwany wynik:** fallback jest dostępny w każdym języku lub przez jawny fallback językowy, bez pustych kroków.

**Pokrycie:** wildcard, PL/EN/UK, kompletność wyników.
