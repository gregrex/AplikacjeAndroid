# Scenariusze produkcyjne — Pies Trener 7 Dni

## SCN-01 — Młody pies z dużą energią

**Cel:** sprawdzić wygenerowanie podstawowego planu treningowego.

**Kroki:**
1. Otwórz projekt `pies-trener-7dni`.
2. Wybierz młodego psa i wysoki poziom energii.
3. Wybierz podstawowy cel treningowy.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja pokazuje siedmiodniowy plan z krótkimi sesjami i uzasadnia dobór intensywności.

**Pokrycie:** reguła szczegółowa, plan 7 dni, wynik free, reason.

## SCN-02 — Dorosły pies i nauka spokojnego zachowania

**Cel:** sprawdzić alternatywny plan.

**Kroki:**
1. Wybierz dorosłego psa.
2. Wybierz spokojny cel treningowy.
3. Otwórz pełny wynik.

**Oczekiwany wynik:** plan różni się od wariantu wysokoenergetycznego i zawiera etapy na kolejne dni.

**Pokrycie:** alternatywna reguła, wynik premium, różnicowanie planu.

## SCN-03 — Niepokojące zachowanie

**Cel:** sprawdzić bezpieczne ograniczenie porad.

**Kroki:**
1. Wybierz odpowiedzi wskazujące silny lęk, agresję lub ból.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja nie proponuje ryzykownego treningu, pokazuje bezpieczny fallback i zaleca konsultację ze specjalistą.

**Pokrycie:** safety fallback, brak diagnozy medycznej.

## SCN-04 — Lokalna analiza dźwięku

**Cel:** sprawdzić ścieżkę audio na telefonie.

**Kroki:**
1. Sprawdź model `local-audio-v1` w ustawieniach.
2. Spróbuj analizy bez modelu.
3. Po instalacji zweryfikowanego modelu wybierz lokalne nagranie do 20 sekund.
4. Potwierdź ręcznie sugerowaną odpowiedź.

**Oczekiwany wynik:** bez modelu analiza jest blokowana; z modelem ONNX wynik jest sugestią i nie zmienia quizu bez potwierdzenia.

**Pokrycie:** audio, downloader, SHA256, ONNX, ręczne potwierdzenie.

## SCN-05 — Historia, ulubione i język

**Cel:** sprawdzić funkcje przekrojowe.

**Kroki:**
1. Wygeneruj plan w języku PL.
2. Dodaj wynik do ulubionych.
3. Sprawdź historię.
4. Zmień język na EN lub UK i ponownie otwórz plan.

**Oczekiwany wynik:** historia i ulubione wskazują właściwy projekt, a wersje językowe zachowują te same `resultId`.

**Pokrycie:** historia, ulubione, PL/EN/UK.
