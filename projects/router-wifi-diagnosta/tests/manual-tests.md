# Router WiFi Diagnosta — manualne testy MVP

## Zakres

Testy obejmują poprawę domowego WiFi na podstawie formularza: problem, typ mieszkania, miejsce routera, liczba ścian i liczba urządzeń.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Router WiFi Diagnosta`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz problem `weak_signal`, typ `flat_large`, miejsce `corner`, ściany `two_three`, urządzenia `few`.
4. Sprawdź, czy wynik premium prowadzi do `weak_signal_place_premium`.
5. Wybierz problem `slow_speed` i sprawdź plan porównania testu blisko routera oraz w problemowym miejscu.
6. Wybierz problem `disconnects` i sprawdź plan stabilności.
7. Wybierz problem `one_room` i sprawdź mapę testów po mieszkaniu.
8. Wybierz problem `many_devices` i sprawdź checklistę obciążenia urządzeniami.
9. Ustaw `router_place = cabinet` i sprawdź ostrzeżenie o złym miejscu routera.
10. Ustaw `walls = many` i sprawdź rekomendację mesh lub punktu dostępowego.

## Testy zasad

1. Sprawdź, czy aplikacja nie zmienia ustawień routera automatycznie.
2. Sprawdź, czy aplikacja nie wymaga logowania do panelu routera.
3. Sprawdź, czy aplikacja zaleca robienie notatek przed zmianami.
4. Sprawdź, czy aplikacja nie zaleca resetu fabrycznego jako pierwszego kroku.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `wifi-blue-diagnostic`.
2. Sprawdź, czy ekran wyniku pokazuje pierwszą rzecz do sprawdzenia i pełny plan po reklamie.
3. Sprawdź, czy wynik można dodać do ulubionych.
4. Sprawdź, czy wynik zapisuje się w historii.
5. Sprawdź, czy pełny plan pokazuje się po reklamie.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją,
- aplikacja daje checklisty i instrukcje, ale nie zmienia konfiguracji routera automatycznie.
