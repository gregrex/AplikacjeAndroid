# Scenariusze produkcyjne — Opis Sprzedażowy

## SCN-01 — Opis odzieży używanej

**Cel:** sprawdzić podstawowe wygenerowanie ogłoszenia.

**Kroki:**
1. Otwórz projekt `vinted-olx-opis`.
2. Wybierz kategorię odzieży.
3. Podaj stan, rozmiar i najważniejszą cechę.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja tworzy czytelny opis sprzedażowy bez dopisywania niepodanych cech.

**Pokrycie:** pytania, reguła szczegółowa, wynik free, rzetelność treści.

## SCN-02 — Przedmiot z widoczną wadą

**Cel:** sprawdzić uczciwy opis stanu.

**Kroki:**
1. Wybierz kategorię produktu.
2. Zaznacz używany stan i istniejącą wadę.
3. Otwórz pełny wynik.

**Oczekiwany wynik:** opis jawnie uwzględnia wadę, nie przedstawia przedmiotu jako nowego i zawiera kompletny szablon premium.

**Pokrycie:** alternatywna reguła, premium, zgodność danych.

## SCN-03 — Niepełne dane produktu

**Cel:** sprawdzić fallback bez halucynowania informacji.

**Kroki:**
1. Wybierz minimalny zestaw odpowiedzi.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja tworzy neutralny szablon z miejscami do uzupełnienia albo prosi o brakujące informacje; nie wymyśla marki, wymiarów ani stanu.

**Pokrycie:** fallback, bezpieczeństwo treści, brak danych.

## SCN-04 — Kopiowanie i zapis opisu

**Cel:** sprawdzić użyteczność wygenerowanej treści.

**Kroki:**
1. Wygeneruj pełny opis.
2. Skopiuj treść do schowka.
3. Dodaj wynik do ulubionych.
4. Sprawdź historię.

**Oczekiwany wynik:** schowek zawiera aktywny opis, a historia i ulubione zapisują poprawny wynik.

**Pokrycie:** clipboard, premium, historia, ulubione.

## SCN-05 — Język i ponowne użycie szablonu

**Cel:** sprawdzić wersje językowe.

**Kroki:**
1. Wygeneruj ten sam typ ogłoszenia w PL, EN i UK.
2. Porównaj strukturę treści i `resultId`.

**Oczekiwany wynik:** identyfikator pozostaje ten sam, a treść jest dostępna w wybranym języku lub przez jawny fallback.

**Pokrycie:** PL/EN/UK, parytet wyników, ponowne użycie.
