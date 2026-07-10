# Scenariusze produkcyjne — Fryzury Proste

## SCN-01 — Szybka fryzura codzienna

**Cel:** sprawdzić podstawową instrukcję uczesania.

**Kroki:**
1. Otwórz projekt `fryzury-proste`.
2. Wybierz krótki czas wykonania i codzienny efekt.
3. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje krótką, wykonalną instrukcję krok po kroku.

**Pokrycie:** pytania, reguła szczegółowa, wynik free.

## SCN-02 — Fryzura na formalne wyjście

**Cel:** sprawdzić wariant elegancki.

**Kroki:**
1. Wybierz formalną okazję.
2. Wybierz eleganckie wykończenie.
3. Otwórz wynik premium.

**Oczekiwany wynik:** wynik różni się od wariantu codziennego i zawiera pełne kroki oraz potrzebne akcesoria.

**Pokrycie:** alternatywna reguła, premium, różnicowanie wyniku.

## SCN-03 — Brak akcesoriów

**Cel:** sprawdzić fallback z ograniczonymi zasobami.

**Kroki:**
1. Zaznacz brak specjalnych akcesoriów.
2. Wybierz nietypową kombinację pozostałych odpowiedzi.
3. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje prostą wersję bazową bez wymagania niedostępnych narzędzi.

**Pokrycie:** fallback, ograniczenia zasobów.

## SCN-04 — Lokalna analiza zdjęcia

**Cel:** sprawdzić rozpoznawanie obrazu na urządzeniu.

**Kroki:**
1. Sprawdź model `local-vision-v1`.
2. Wybierz lokalne zdjęcie fryzury po instalacji modelu.
3. Uruchom analizę.
4. Potwierdź ręcznie sugerowane wykończenie.

**Oczekiwany wynik:** ONNX zwraca sugestię albo czytelny błąd zgodności modelu; aplikacja nie zatwierdza odpowiedzi automatycznie.

**Pokrycie:** obraz, LocalFilePath, ONNX, potwierdzenie użytkownika.

## SCN-05 — Ulubione, historia i język

**Cel:** sprawdzić funkcje trwałe.

**Kroki:**
1. Wygeneruj pełną instrukcję.
2. Dodaj ją do ulubionych.
3. Sprawdź historię.
4. Zmień język na EN lub UK.

**Oczekiwany wynik:** wynik jest zapisany z poprawnym `ProjectId`, a wersje językowe zachowują ten sam `resultId`.

**Pokrycie:** ulubione, historia, PL/EN/UK.
