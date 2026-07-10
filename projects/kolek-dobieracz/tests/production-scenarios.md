# Scenariusze produkcyjne — Kołek Dobieracz

## SCN-01 — Lekki przedmiot na płycie g-k

**Cel:** sprawdzić podstawowy dobór mocowania.

**Kroki:**
1. Otwórz projekt `kolek-dobieracz`.
2. Wybierz płytę g-k.
3. Wybierz lekki przedmiot.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje mocowanie odpowiednie do lekkiego obciążenia i pokazuje uzasadnienie wyboru.

**Pokrycie:** pytania, reguła szczegółowa, wynik free, uzasadnienie.

## SCN-02 — Ciężki przedmiot na betonie

**Cel:** sprawdzić scenariusz dużego obciążenia.

**Kroki:**
1. Wybierz beton.
2. Wybierz ciężki przedmiot.
3. Przejdź do wyniku premium.

**Oczekiwany wynik:** wynik zawiera mocowanie do betonu, kontrolę nośności i ostrzeżenie przed montażem bez pewności co do podłoża.

**Pokrycie:** duże obciążenie, premium, bezpieczeństwo.

## SCN-03 — Nieznany materiał ściany

**Cel:** sprawdzić bezpieczny fallback.

**Kroki:**
1. Wybierz odpowiedź „nie wiem” lub kombinację bez reguły szczegółowej.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja nie zgaduje rodzaju ściany, zaleca identyfikację podłoża i nie podaje ryzykownego montażu.

**Pokrycie:** fallback, brak danych, bezpieczeństwo.

## SCN-04 — Alternatywna rekomendacja i zapis

**Cel:** sprawdzić alternatywy oraz trwałość danych.

**Kroki:**
1. Wygeneruj wynik z kilkoma pasującymi regułami.
2. Odblokuj premium.
3. Przełącz rekomendację alternatywną.
4. Dodaj ją do ulubionych.

**Oczekiwany wynik:** wybrana alternatywa staje się aktywna, a do ulubionych trafia jej właściwy `ResultId`.

**Pokrycie:** alternatywy, premium, ulubione.

## SCN-05 — Języki i restart aplikacji

**Cel:** sprawdzić lokalizację i odtworzenie projektu.

**Kroki:**
1. Ustaw kolejno PL, EN i UK.
2. Wygeneruj ten sam wynik w każdym języku.
3. Zamknij i uruchom aplikację ponownie.

**Oczekiwany wynik:** identyfikator wyniku pozostaje ten sam, treść korzysta z wybranego języka lub jawnego fallbacku, a aplikacja uruchamia się bez błędu.

**Pokrycie:** PL/EN/UK, fallback językowy, stabilność.
