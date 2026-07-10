# Scenariusze produkcyjne — Pakowanie Paczek

## SCN-01 — Szkło lub ceramika

**Cel:** sprawdzić podstawową checklistę dla przedmiotu kruchego.

**Kroki:**
1. Otwórz projekt `pakowanie-paczek`.
2. Wybierz szkło lub ceramikę.
3. Wybierz standardową przesyłkę kurierską.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje zabezpieczenie wielowarstwowe, wypełnienie pustych przestrzeni i test potrząsania.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Elektronika

**Cel:** sprawdzić alternatywną checklistę.

**Kroki:**
1. Wybierz elektronikę.
2. Zaznacz brak oryginalnego opakowania.
3. Otwórz wynik premium.

**Oczekiwany wynik:** wynik uwzględnia ochronę przed uderzeniami, wilgocią i przemieszczaniem się urządzenia.

**Pokrycie:** alternatywna reguła, premium, kompletność zabezpieczeń.

## SCN-03 — Ubrania lub miękki przedmiot

**Cel:** sprawdzić prostszy wariant pakowania.

**Kroki:**
1. Wybierz odzież.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja nie proponuje nadmiarowego zabezpieczenia kruchego, ale uwzględnia ochronę przed wilgocią i rozerwaniem.

**Pokrycie:** różnicowanie kategorii, wynik.

## SCN-04 — Przedmiot ciężki lub nietypowy

**Cel:** sprawdzić bezpieczny fallback.

**Kroki:**
1. Wybierz duży ciężar lub nietypowy kształt.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja nie udaje pewności, zaleca mocniejsze opakowanie lub usługę specjalistyczną i nie zwraca pustej checklisty.

**Pokrycie:** fallback, bezpieczeństwo transportu.

## SCN-05 — Premium, historia i język

**Cel:** sprawdzić funkcje przekrojowe.

**Kroki:**
1. Odblokuj pełną checklistę.
2. Dodaj ją do ulubionych i sprawdź historię.
3. Otwórz wynik w EN i UK.

**Oczekiwany wynik:** zapis zawiera aktywny wynik, a wersje językowe zachowują ten sam `resultId`.

**Pokrycie:** premium, ulubione, historia, PL/EN/UK.
