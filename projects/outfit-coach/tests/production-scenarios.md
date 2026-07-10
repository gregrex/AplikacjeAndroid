# Scenariusze produkcyjne — Outfit Coach

## SCN-01 — Formalne wyjście w chłodny dzień

**Cel:** sprawdzić podstawowy dobór stroju.

**Kroki:**
1. Otwórz projekt `outfit-coach`.
2. Wybierz formalną okazję i chłodną pogodę.
3. Wybierz styl smart lub klasyczny.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje spójny, warstwowy zestaw i wyjaśnia dopasowanie do okazji i pogody.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Casual na ciepły dzień

**Cel:** sprawdzić alternatywny wariant.

**Kroki:**
1. Wybierz codzienną okazję i ciepłą pogodę.
2. Otwórz wynik premium.

**Oczekiwany wynik:** zestaw jest lżejszy od formalnego, a premium zawiera pełną checklistę elementów.

**Pokrycie:** alternatywna reguła, premium, różnicowanie wyników.

## SCN-03 — Niepełne dane lub nietypowa kombinacja

**Cel:** sprawdzić neutralny fallback.

**Kroki:**
1. Wybierz kombinację bez dedykowanej reguły.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje neutralny zestaw bazowy, nie ocenia osoby i nie zwraca pustego wyniku.

**Pokrycie:** fallback, neutralny język, wynik domyślny.

## SCN-04 — Lokalna analiza zdjęcia stroju

**Cel:** sprawdzić Local AI image.

**Kroki:**
1. Sprawdź status modelu `local-vision-v1`.
2. Spróbuj analizy bez modelu.
3. Po instalacji wybierz lokalne zdjęcie stroju.
4. Potwierdź ręcznie sugerowany styl.

**Oczekiwany wynik:** bez modelu analiza jest blokowana; z modelem ONNX sugestia nie jest stosowana bez potwierdzenia.

**Pokrycie:** obraz, ONNX, downloader, prywatność lokalna.

## SCN-05 — Alternatywy, ulubione i język

**Cel:** sprawdzić funkcje przekrojowe.

**Kroki:**
1. Wygeneruj wynik z alternatywną rekomendacją.
2. Przełącz alternatywę i dodaj ją do ulubionych.
3. Zmień język na EN lub UK.

**Oczekiwany wynik:** zapisywany jest aktywny `ResultId`, a wersje językowe zachowują parytet wyników.

**Pokrycie:** alternatywy, ulubione, PL/EN/UK.
