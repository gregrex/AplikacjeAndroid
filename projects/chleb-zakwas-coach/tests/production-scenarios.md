# Scenariusze produkcyjne — Chleb Zakwas Coach

## SCN-01 — Słaby zakwas

**Cel:** sprawdzić podstawową diagnozę zakwasu.

**Kroki:**
1. Otwórz projekt `chleb-zakwas-coach`.
2. Wybierz małą aktywność i niewielki przyrost objętości.
3. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje plan dokarmiania i temperatury oraz wyjaśnia dobór zaleceń.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Zbity chleb

**Cel:** sprawdzić korektę procesu pieczenia.

**Kroki:**
1. Wybierz zbity miękisz i małą objętość bochenka.
2. Otwórz wynik premium.

**Oczekiwany wynik:** wynik obejmuje fermentację, nawodnienie i kontrolę wyrastania, a kroki są ułożone w logicznej kolejności.

**Pokrycie:** alternatywna reguła, premium, diagnostyka procesu.

## SCN-03 — Przerośnięte lub niedorośnięte ciasto

**Cel:** sprawdzić różnicowanie podobnych objawów.

**Kroki:**
1. Wykonaj quiz dla niedorośniętego ciasta.
2. Powtórz dla przerośniętego ciasta.

**Oczekiwany wynik:** aplikacja zwraca różne rekomendacje i uzasadnia, które odpowiedzi wpłynęły na wybór.

**Pokrycie:** dwie reguły, matched conditions, reason.

## SCN-04 — Podejrzenie pleśni lub zepsucia

**Cel:** sprawdzić scenariusz bezpieczeństwa żywności.

**Kroki:**
1. Wybierz odpowiedzi wskazujące kolorowy nalot, nietypowy zapach lub pleśń.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja nie zaleca dalszego używania zakwasu i pokazuje bezpieczne zalecenie jego wyrzucenia.

**Pokrycie:** safety fallback, żywność, brak ryzykownej porady.

## SCN-05 — Historia, ulubione i języki

**Cel:** sprawdzić funkcje przekrojowe.

**Kroki:**
1. Zapisz pełną rekomendację do ulubionych.
2. Sprawdź historię.
3. Otwórz wynik w PL, EN i UK.

**Oczekiwany wynik:** zapis zawiera właściwy projekt i wynik, a wersje językowe mają ten sam `resultId`.

**Pokrycie:** historia, ulubione, PL/EN/UK.
