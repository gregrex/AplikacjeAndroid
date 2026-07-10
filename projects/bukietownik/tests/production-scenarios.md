# Scenariusze produkcyjne — Bukietownik

## SCN-01 — Bukiet urodzinowy

**Cel:** sprawdzić podstawową kompozycję.

**Kroki:**
1. Otwórz projekt `bukietownik`.
2. Wybierz okazję urodzinową.
3. Wybierz średni budżet i kolorowy styl.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje spójną kompozycję i wyjaśnia dobór kwiatów oraz kolorów.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Bukiet na formalną okazję

**Cel:** sprawdzić stonowany wariant.

**Kroki:**
1. Wybierz formalną okazję.
2. Wybierz spokojną kolorystykę.
3. Otwórz wynik premium.

**Oczekiwany wynik:** kompozycja różni się od urodzinowej i zawiera pełną listę elementów oraz układania.

**Pokrycie:** alternatywna reguła, premium, różnicowanie stylu.

## SCN-03 — Bardzo mały budżet

**Cel:** sprawdzić rekomendację przy ograniczonych zasobach.

**Kroki:**
1. Wybierz minimalny budżet.
2. Zakończ quiz.

**Oczekiwany wynik:** wynik proponuje prostszą kompozycję bez przekraczania zadeklarowanego budżetu.

**Pokrycie:** ograniczenie budżetu, dopasowanie wyniku.

## SCN-04 — Fallback dla nietypowej okazji

**Cel:** sprawdzić wynik domyślny.

**Kroki:**
1. Wybierz kombinację bez szczegółowej reguły.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje neutralny bukiet bazowy i nie zwraca pustej listy kroków.

**Pokrycie:** fallback wildcard, kompletność wyniku.

## SCN-05 — Zapis i języki

**Cel:** sprawdzić trwałość i lokalizację.

**Kroki:**
1. Dodaj pełny wynik do ulubionych.
2. Sprawdź historię.
3. Otwórz wynik w PL, EN i UK.

**Oczekiwany wynik:** wpisy zawierają poprawny `ProjectId`, a `resultId` pozostaje spójny między językami.

**Pokrycie:** ulubione, historia, PL/EN/UK.
