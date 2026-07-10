# Scenariusze produkcyjne — Rysunek Coach

## SCN-01 — Zwierzę dla początkującego

**Cel:** sprawdzić podstawową lekcję rysowania.

**Kroki:**
1. Otwórz projekt `rysunek-coach`.
2. Wybierz poziom początkujący i temat zwierzęcia.
3. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje prostą lekcję krok po kroku z uzasadnieniem poziomu trudności.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Przedmiot na poziomie średnim

**Cel:** sprawdzić trudniejszy wariant lekcji.

**Kroki:**
1. Wybierz poziom średni i temat przedmiotu.
2. Otwórz wynik premium.

**Oczekiwany wynik:** instrukcja ma więcej etapów i różni się od wariantu początkującego.

**Pokrycie:** alternatywna reguła, premium, różnicowanie poziomu.

## SCN-03 — Nietypowa kombinacja odpowiedzi

**Cel:** sprawdzić fallback.

**Kroki:**
1. Wybierz kombinację bez reguły szczegółowej.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje bazowe ćwiczenie rysunkowe i nie zwraca pustego wyniku.

**Pokrycie:** wildcard fallback, wynik domyślny.

## SCN-04 — Lokalna analiza szkicu

**Cel:** sprawdzić analizę obrazu ONNX.

**Kroki:**
1. Sprawdź i pobierz model `local-vision-v1`.
2. Wybierz lokalny obraz szkicu.
3. Uruchom analizę.
4. Potwierdź sugerowany temat ręcznie.

**Oczekiwany wynik:** aplikacja używa `LocalFilePath`, uruchamia lokalną inferencję albo zgłasza czytelny błąd modelu, bez automatycznego zatwierdzania odpowiedzi.

**Pokrycie:** obraz, ONNX, downloader, ręczne potwierdzenie.

## SCN-05 — Historia postępu i język

**Cel:** sprawdzić zapis lekcji i lokalizację.

**Kroki:**
1. Wygeneruj pełną lekcję.
2. Dodaj ją do ulubionych i sprawdź historię.
3. Otwórz ten sam wynik w EN i UK.

**Oczekiwany wynik:** zapis wskazuje właściwy projekt, a `resultId` jest spójny między językami.

**Pokrycie:** historia, ulubione, PL/EN/UK.
