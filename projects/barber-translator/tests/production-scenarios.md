# Scenariusze produkcyjne — Barber Translator

## SCN-01 — Krótka góra i mid fade

**Cel:** sprawdzić podstawowy brief do barbera.

**Kroki:**
1. Otwórz projekt `barber-translator`.
2. Wybierz krótką górę i mid fade.
3. Zakończ quiz.

**Oczekiwany wynik:** aplikacja tworzy jasny opis długości i przejścia oraz pokazuje uzasadnienie.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Kręcone włosy

**Cel:** sprawdzić wariant zachowania tekstury.

**Kroki:**
1. Wybierz włosy kręcone.
2. Zaznacz zachowanie loków.
3. Otwórz pełny wynik.

**Oczekiwany wynik:** brief nie proponuje agresywnego skracania loków i zawiera kompletną instrukcję premium.

**Pokrycie:** alternatywna reguła, premium, tekstura włosów.

## SCN-03 — Zachowanie długości po bokach

**Cel:** sprawdzić ograniczenie zakresu strzyżenia.

**Kroki:**
1. Zaznacz zachowanie długości po bokach.
2. Zakończ quiz.

**Oczekiwany wynik:** wynik wyraźnie ogranicza skracanie boków i nie wybiera ostrego fade jako głównej rekomendacji.

**Pokrycie:** warunek negatywny, dopasowanie reguły.

## SCN-04 — Lokalna analiza zdjęcia

**Cel:** sprawdzić ścieżkę obrazu ONNX.

**Kroki:**
1. Sprawdź model `local-vision-v1`.
2. Bez modelu spróbuj analizy zdjęcia fryzury.
3. Po instalacji modelu wybierz lokalny plik obrazu.
4. Potwierdź ręcznie sugerowany styl.

**Oczekiwany wynik:** brak modelu blokuje analizę; zweryfikowany model zwraca sugestię bez automatycznego zatwierdzenia.

**Pokrycie:** obraz, downloader, ONNX, ręczne potwierdzenie.

## SCN-05 — Kopiowanie i języki

**Cel:** sprawdzić użycie briefu poza aplikacją.

**Kroki:**
1. Wygeneruj pełny brief.
2. Skopiuj go do schowka.
3. Powtórz w PL, EN i UK.

**Oczekiwany wynik:** schowek zawiera aktywny brief, a identyfikator wyniku pozostaje spójny między językami.

**Pokrycie:** clipboard, premium, PL/EN/UK.
