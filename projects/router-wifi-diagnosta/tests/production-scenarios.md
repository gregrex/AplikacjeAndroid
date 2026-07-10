# Scenariusze produkcyjne — Router WiFi Diagnosta

## SCN-01 — Słaby zasięg w jednym pokoju

**Cel:** sprawdzić podstawową diagnostykę zasięgu.

**Kroki:**
1. Otwórz projekt `router-wifi-diagnosta`.
2. Wybierz słaby sygnał w odległym pokoju.
3. Zaznacz działający internet przy routerze.
4. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje zmianę położenia routera, kontrolę przeszkód i kanału oraz pokazuje uzasadnienie.

**Pokrycie:** reguła szczegółowa, wynik free, reason.

## SCN-02 — Wolne WiFi na wielu urządzeniach

**Cel:** sprawdzić wariant wydajnościowy.

**Kroki:**
1. Wybierz wolną prędkość na kilku urządzeniach.
2. Otwórz wynik premium.

**Oczekiwany wynik:** wynik zawiera kontrolę pasma, obciążenia, test przy routerze i porównanie z łączem operatora.

**Pokrycie:** alternatywna reguła, premium, diagnostyka wydajności.

## SCN-03 — Duże mieszkanie i martwe strefy

**Cel:** sprawdzić rekomendację mesh lub access point.

**Kroki:**
1. Wybierz duży obszar i kilka martwych stref.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja proponuje mesh lub access point dopiero po podstawowych kontrolach i wyjaśnia wybór.

**Pokrycie:** reguła mesh/AP, kolejność rekomendacji.

## SCN-04 — Brak internetu przy poprawnym WiFi

**Cel:** sprawdzić rozróżnienie WiFi od łącza operatora.

**Kroki:**
1. Wybierz połączenie z siecią bez dostępu do internetu.
2. Zakończ quiz.

**Oczekiwany wynik:** aplikacja nie traktuje problemu wyłącznie jako zasięgu, zaleca kontrolę modemu/operatora i pokazuje bezpieczny fallback.

**Pokrycie:** diagnostyka przyczyny, fallback, brak błędnej rekomendacji.

## SCN-05 — Alternatywy, historia i języki

**Cel:** sprawdzić funkcje przekrojowe.

**Kroki:**
1. Wygeneruj wynik z alternatywami.
2. Odblokuj premium i wybierz alternatywę.
3. Dodaj wynik do ulubionych oraz sprawdź historię.
4. Otwórz wynik w EN i UK.

**Oczekiwany wynik:** aktywny `ResultId` jest zapisany, a wersje językowe zachowują parytet i komplet kroków.

**Pokrycie:** alternatywy, premium, historia, ulubione, PL/EN/UK.
