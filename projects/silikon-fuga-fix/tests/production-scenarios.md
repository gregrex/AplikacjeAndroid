# Scenariusze produkcyjne — Silikon Fuga Fix

## SCN-01 — Pleśń na silikonie

**Cel:** sprawdzić scenariusz safety-sensitive.

**Kroki:**
1. Otwórz projekt `silikon-fuga-fix`.
2. Wybierz ciemny nalot lub podejrzenie pleśni.
3. Zakończ quiz.

**Oczekiwany wynik:** aplikacja zaleca wentylację, rękawice i niemieszanie środków chemicznych oraz pokazuje uzasadnienie.

**Pokrycie:** safety, reguła szczegółowa, wynik free, reason.

## SCN-02 — Pęknięty silikon

**Cel:** sprawdzić wariant wymiany.

**Kroki:**
1. Wybierz pęknięcie lub odspojenie silikonu.
2. Otwórz wynik premium.

**Oczekiwany wynik:** wynik prowadzi przez ocenę, usunięcie starego materiału, przygotowanie i ponowne nałożenie w bezpiecznej kolejności.

**Pokrycie:** alternatywna reguła, premium, kolejność procesu.

## SCN-03 — Zwykłe zabrudzenie fugi

**Cel:** sprawdzić łagodniejszy wariant.

**Kroki:**
1. Wybierz powierzchowne zabrudzenie bez uszkodzeń.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje czyszczenie zamiast niepotrzebnej wymiany i nie zaleca agresywnej chemii bez testu.

**Pokrycie:** różnicowanie problemu, bezpieczeństwo środków.

## SCN-04 — Lokalna analiza zdjęcia

**Cel:** sprawdzić obraz ONNX w projekcie safety-sensitive.

**Kroki:**
1. Sprawdź model `local-vision-v1`.
2. Wybierz lokalne zdjęcie fugi lub silikonu.
3. Uruchom analizę.
4. Potwierdź ręcznie sugerowany typ problemu.

**Oczekiwany wynik:** sugestia nie zastępuje oceny ryzyka, a bez modelu lub przy błędzie ONNX pojawia się czytelna blokada.

**Pokrycie:** obraz, ONNX, safety-sensitive warning, ręczne potwierdzenie.

## SCN-05 — Fallback i zapis

**Cel:** sprawdzić nietypowy problem oraz trwałość.

**Kroki:**
1. Wybierz kombinację bez reguły szczegółowej.
2. Otwórz wynik domyślny.
3. Dodaj go do ulubionych i sprawdź historię.

**Oczekiwany wynik:** aplikacja pokazuje bezpieczną procedurę oceny, a zapis zawiera właściwy projekt i wynik.

**Pokrycie:** fallback, ulubione, historia.
