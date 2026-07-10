# Scenariusze produkcyjne — Plama Ratownik

## SCN-01 — Świeża kawa na bawełnie

**Cel:** sprawdzić podstawowy przebieg od kategorii do rekomendacji.

**Kroki:**
1. Otwórz projekt `plama-ratownik`.
2. Wybierz kategorię kawy.
3. Wybierz bawełnę i świeżą plamę.
4. Zakończ quiz i otwórz wynik.

**Oczekiwany wynik:** pojawia się dopasowana instrukcja podstawowa, sekcja „Dlaczego taki wynik?” oraz prawidłowe ostrzeżenia dotyczące temperatury i suszenia.

**Pokrycie:** katalog, pytania, reguła szczegółowa, wynik free, uzasadnienie.

## SCN-02 — Stara plama krwi

**Cel:** sprawdzić scenariusz bezpieczeństwa dla plamy utrwalonej.

**Kroki:**
1. Wybierz kategorię krwi.
2. Zaznacz starą plamę.
3. Przejdź do wyniku.
4. Odblokuj pełną instrukcję.

**Oczekiwany wynik:** wynik zaleca zimną wodę, nie zaleca gorącej wody i pokazuje pełne kroki po odblokowaniu premium.

**Pokrycie:** reguła bezpieczeństwa, wynik premium, rewarded mock, ostrzeżenia.

## SCN-03 — Nieznany typ plamy

**Cel:** sprawdzić bezpieczny fallback.

**Kroki:**
1. Wybierz kategorię lub kombinację odpowiedzi bez reguły szczegółowej.
2. Zakończ quiz.
3. Otwórz wynik.

**Oczekiwany wynik:** aplikacja pokazuje ogólną procedurę, nie zwraca pustego ekranu i nie proponuje mieszania przypadkowych środków chemicznych.

**Pokrycie:** fallback wildcard, wynik domyślny, bezpieczeństwo.

## SCN-04 — Historia i ulubione

**Cel:** sprawdzić trwałość wyniku.

**Kroki:**
1. Wygeneruj dowolny wynik.
2. Odblokuj pełną instrukcję.
3. Dodaj wynik do ulubionych.
4. Otwórz historię i ulubione.

**Oczekiwany wynik:** wynik występuje w historii oraz ulubionych z poprawnym `ProjectId` i `ResultId`.

**Pokrycie:** premium, historia, ulubione, identyfikacja projektu.

## SCN-05 — Lokalna analiza zdjęcia

**Cel:** sprawdzić przepływ Local AI bez udawanego wyniku.

**Kroki:**
1. Otwórz ustawienia i sprawdź status modelu `local-vision-v1`.
2. Bez skonfigurowanego modelu spróbuj analizować zdjęcie plamy.
3. Po skonfigurowaniu modelu wybierz lokalny plik JPEG i uruchom analizę.
4. Potwierdź lub odrzuć sugerowane odpowiedzi ręcznie.

**Oczekiwany wynik:** bez modelu analiza jest blokowana czytelnym komunikatem; ze zweryfikowanym modelem ONNX wynik jest sugestią i nie omija ręcznego potwierdzenia.

**Pokrycie:** pobieranie modelu, SHA256, ONNX, LocalFilePath, safety-sensitive UX.
