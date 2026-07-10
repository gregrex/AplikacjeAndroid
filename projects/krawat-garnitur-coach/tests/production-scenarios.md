# Scenariusze produkcyjne — Krawat Garnitur Coach

## SCN-01 — Ślub formalny

**Cel:** sprawdzić podstawowy dobór formalnego zestawu.

**Kroki:**
1. Otwórz projekt `krawat-garnitur-coach`.
2. Wybierz ślub lub bardzo formalną okazję.
3. Wybierz klasyczny garnitur i krawat.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje spójny zestaw i wyjaśnia dopasowanie poziomu formalności.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Spotkanie biznesowe

**Cel:** sprawdzić wariant biznesowy.

**Kroki:**
1. Wybierz spotkanie biznesowe.
2. Wybierz stonowaną kolorystykę.
3. Otwórz wynik premium.

**Oczekiwany wynik:** rekomendacja różni się od ślubnej i zawiera pełną checklistę koszuli, krawata, butów i dodatków.

**Pokrycie:** alternatywna reguła, premium, kompletność zestawu.

## SCN-03 — Zestaw bez krawata

**Cel:** sprawdzić mniej formalny wariant.

**Kroki:**
1. Wybierz okazję smart casual.
2. Zaznacz brak krawata.
3. Zakończ quiz.

**Oczekiwany wynik:** aplikacja nie wymusza krawata, a rekomendacja pozostaje spójna z wybraną formalnością.

**Pokrycie:** warunek opcjonalny, różnicowanie formalności.

## SCN-04 — Nietypowe kolory

**Cel:** sprawdzić neutralny fallback.

**Kroki:**
1. Wybierz nietypową kombinację kolorów bez dedykowanej reguły.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje neutralną bazę i nie ocenia wyglądu użytkownika ani jego cech osobistych.

**Pokrycie:** fallback, neutralny język, bezpieczeństwo UX.

## SCN-05 — Ulubione i języki

**Cel:** sprawdzić zapis i parytet językowy.

**Kroki:**
1. Dodaj pełny wynik do ulubionych.
2. Sprawdź historię.
3. Otwórz wynik w PL, EN i UK.

**Oczekiwany wynik:** zapis wskazuje właściwy projekt i aktywny wynik, a `resultId` pozostaje spójny między językami.

**Pokrycie:** ulubione, historia, PL/EN/UK.
