# Kot Bawi się — manualne testy MVP

## Zakres

Testy obejmują dane, runtime, języki, ścieżki wyniku, bezpieczeństwo komunikatów, marketing i integrację z katalogiem projektów.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Kot Bawi się` z katalogu.
2. Sprawdź, czy ekran startowy pokazuje nazwę i opis projektu.
3. Wybierz kategorię `kitten`.
4. Odpowiedz: wiek `kitten`, energia `high`, czas `10`, zabawka `fishing_rod`.
5. Sprawdź, czy wynik premium prowadzi do `kitten_high_premium`.
6. Wybierz kategorię `senior`.
7. Odpowiedz: wiek `senior`, energia `low`, czas `5`, zabawka `no_toys`.
8. Sprawdź, czy wynik prowadzi do planu bez skoków i nadmiernego wysiłku.
9. Wybierz kategorię `diy` i zabawkę `cardboard`.
10. Sprawdź, czy wynik zawiera instrukcję kartonowej kryjówki.
11. Wybierz zabawkę `food`.
12. Sprawdź, czy wynik zawiera zabawę węchową z karmą.
13. Użyj czasu `5`.
14. Sprawdź, czy fallback mikrosesji działa, gdy brak silniejszej reguły.
15. Uruchom wynik domyślny dla kategorii `indoor` bez szczególnych dopasowań.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników po polsku.
2. Ustaw język EN i sprawdź, czy identyfikatory wyników są takie same jak w PL.
3. Ustaw język UK i sprawdź, czy identyfikatory wyników są takie same jak w PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i co najmniej jeden krok.

## Testy bezpieczeństwa treści

1. Sprawdź, czy aplikacja nie diagnozuje chorób kota.
2. Sprawdź, czy przy bólu, kuleniu lub nagłej zmianie zachowania tekst kieruje do weterynarza.
3. Sprawdź, czy aplikacja ostrzega przed zabawą rękami.
4. Sprawdź, czy aplikacja ostrzega przed ostrymi elementami w kartonie.
5. Sprawdź, czy aplikacja nie zachęca do wymuszania aktywności.

## Testy monetyzacji

1. Sprawdź wynik darmowy dla każdej reguły.
2. Sprawdź wynik premium po reklamie dla każdej reguły.
3. Sprawdź, czy wynik premium zawiera `needed` lub `dontDo`, gdy jest to potrzebne.
4. Sprawdź, czy historia zapisuje ostatnio wybrany wynik.
5. Sprawdź, czy wynik można dodać do ulubionych.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją.
