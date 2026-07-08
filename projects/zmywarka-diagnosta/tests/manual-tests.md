# Zmywarka Diagnosta — manualne testy MVP

## Zakres

Testy obejmują typowe objawy zmywarki: nie domywa, zostawia osad, brzydko pachnie, tabletka się nie rozpuszcza, naczynia są mokre, nie odpompowuje wody, pokazuje błąd oraz scenariusze bezpieczeństwa.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Zmywarka Diagnosta`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz `not_cleaning`, częstotliwość `always`, filtr `dirty`, bezpieczeństwo `normal`.
4. Sprawdź, czy wynik prowadzi do `not_cleaning_filter_premium`.
5. Wybierz `residue` i sprawdź sól oraz nabłyszczacz.
6. Wybierz `bad_smell` i sprawdź filtr, uszczelki oraz ostrzeżenie o zapachu spalenizny.
7. Wybierz `tablet_not_dissolved` i sprawdź dozownik, ramiona oraz program.
8. Wybierz `wet_dishes` i sprawdź nabłyszczacz oraz program suszenia.
9. Wybierz `not_draining` i sprawdź, czy aplikacja zaczyna od filtra i nie prowadzi przez pompę.
10. Wybierz `error_code` i sprawdź, czy aplikacja każe zapisać kod oraz sprawdzić instrukcję.

## Testy bezpieczeństwa

1. Ustaw `safety = leak`.
2. Sprawdź, czy wynik prowadzi do `safety_leak_stop_premium`.
3. Ustaw `safety = burning_smell`.
4. Sprawdź, czy wynik prowadzi do `safety_burning_stop_premium`.
5. Ustaw `safety = electric_risk`.
6. Sprawdź, czy wynik prowadzi do `safety_electric_stop_premium`.
7. Sprawdź, czy aplikacja nie prowadzi przez naprawy elektryczne, pompę, moduł sterujący ani rozbieranie urządzenia.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `dishwasher-clean-safe`.
2. Sprawdź, czy ekran wyniku pokazuje pierwszy krok, pełną checklistę i ostrzeżenia.
3. Sprawdź, czy wynik można dodać do ulubionych.
4. Sprawdź, czy wynik zapisuje się w historii.
5. Sprawdź, czy pełna checklista pokazuje się po reklamie.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją,
- wyciek, spalenizna i ryzyko porażenia zawsze kończą się stopem i serwisem.
