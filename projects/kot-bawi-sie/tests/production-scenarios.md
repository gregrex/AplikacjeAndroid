# Scenariusze produkcyjne — Kot Bawi się

## SCN-01 — Kociak z dużą energią

**Cel:** sprawdzić podstawową rekomendację zabawy.

**Kroki:**
1. Otwórz projekt `kot-bawi-sie`.
2. Wybierz młodego kota i wysoki poziom energii.
3. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje krótkie, bezpieczne sesje ruchowe i wyjaśnia wybór.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Kot senior

**Cel:** sprawdzić spokojny wariant aktywności.

**Kroki:**
1. Wybierz kota seniora.
2. Wybierz niski poziom energii.
3. Otwórz wynik premium.

**Oczekiwany wynik:** rekomendacja jest łagodna, nie wymaga wysokich skoków i zawiera pełny plan.

**Pokrycie:** alternatywna reguła, premium, dopasowanie wieku.

## SCN-03 — Zabawa z jedzeniem

**Cel:** sprawdzić scenariusz łamigłówki żywieniowej.

**Kroki:**
1. Wybierz smaczki lub karmę jako dostępny zasób.
2. Zakończ quiz.

**Oczekiwany wynik:** wynik proponuje kontrolowaną zabawę food puzzle i nie zachęca do przekarmiania.

**Pokrycie:** reguła food puzzle, ostrzeżenia, wynik.

## SCN-04 — Lokalna analiza dźwięku

**Cel:** sprawdzić audio AI.

**Kroki:**
1. Sprawdź model `local-audio-v1`.
2. Spróbuj analizy bez modelu.
3. Po instalacji modelu wybierz nagranie do 20 sekund.
4. Potwierdź ręcznie sugestię energii kota.

**Oczekiwany wynik:** brak modelu blokuje analizę; poprawny model ONNX zwraca sugestię bez automatycznej zmiany quizu.

**Pokrycie:** audio, ONNX, downloader, ręczne potwierdzenie.

## SCN-05 — Historia, ulubione i fallback

**Cel:** sprawdzić funkcje przekrojowe.

**Kroki:**
1. Wybierz kombinację bez szczegółowej reguły.
2. Zapisz wynik domyślny do ulubionych.
3. Sprawdź historię.

**Oczekiwany wynik:** aplikacja pokazuje bezpieczną zabawę domową, a zapis wskazuje właściwy projekt i wynik.

**Pokrycie:** fallback, historia, ulubione.
