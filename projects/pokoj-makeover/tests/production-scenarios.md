# Scenariusze produkcyjne — Pokój Makeover

## SCN-01 — Przytulny pokój bez budżetu

**Cel:** sprawdzić podstawową rekomendację metamorfozy.

**Kroki:**
1. Otwórz projekt `pokoj-makeover`.
2. Wybierz przytulny styl.
3. Zaznacz budżet zero.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje zmianę układu, światła i tekstyliów bez zakupów oraz pokazuje uzasadnienie.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Mały pokój i przechowywanie

**Cel:** sprawdzić wariant dla ograniczonej przestrzeni.

**Kroki:**
1. Wybierz mały pokój.
2. Wskaż problem z przechowywaniem.
3. Otwórz wynik premium.

**Oczekiwany wynik:** rekomendacja wykorzystuje przestrzeń pionową i zawiera pełny plan etapów.

**Pokrycie:** alternatywna reguła, premium, ograniczenia przestrzeni.

## SCN-03 — Strefa gamingowa

**Cel:** sprawdzić dedykowany wariant funkcjonalny.

**Kroki:**
1. Wybierz gaming jako główną funkcję pokoju.
2. Zakończ quiz.

**Oczekiwany wynik:** wynik uwzględnia biurko, kable, oświetlenie i wygodę bez proponowania remontu.

**Pokrycie:** reguła gaming, kompletność wyniku.

## SCN-04 — Lokalna analiza zdjęcia pokoju

**Cel:** sprawdzić analizę obrazu na urządzeniu.

**Kroki:**
1. Sprawdź model `local-vision-v1`.
2. Wybierz lokalne zdjęcie pokoju po instalacji modelu.
3. Uruchom analizę.
4. Potwierdź ręcznie sugerowany styl lub budżet.

**Oczekiwany wynik:** analiza działa lokalnie przez ONNX albo zwraca czytelny błąd zgodności modelu; sugestia nie jest zatwierdzana automatycznie.

**Pokrycie:** obraz, ONNX, LocalFilePath, ręczne potwierdzenie.

## SCN-05 — Alternatywy i zapis

**Cel:** sprawdzić wiele pasujących rekomendacji.

**Kroki:**
1. Wygeneruj wynik z alternatywami.
2. Odblokuj premium.
3. Przełącz aktywną alternatywę.
4. Dodaj ją do ulubionych i sprawdź historię.

**Oczekiwany wynik:** zapisany zostaje aktywny `ResultId`, a historia wskazuje właściwy projekt.

**Pokrycie:** alternatywy, premium, ulubione, historia.
