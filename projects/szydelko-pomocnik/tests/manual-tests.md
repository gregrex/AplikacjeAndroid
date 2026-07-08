# Szydełko Pomocnik — manualne testy MVP

## Zakres

Testy obejmują offline prowadzenie projektu szydełkowego: licznik rzędów, notatki, skróty, podstawowe ściegi, materiały i proste wzory.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Szydełko Pomocnik`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `scarf`.
4. Ustaw: projekt `scarf`, trudność `beginner`, szydełko `medium_hook`, włóczka `acrylic`, potrzeba `basic_pattern`.
5. Sprawdź, czy wynik premium prowadzi do `scarf_beginner_premium`.
6. Ustaw potrzebę `row_counter`.
7. Sprawdź, czy wynik prowadzi do `row_counter_premium`.
8. Wybierz kategorię `abbreviations` i potrzebę `abbrev`.
9. Sprawdź, czy wynik prowadzi do `abbrev_dictionary_premium`.
10. Wybierz kategorię `stitches` i potrzebę `stitch_help`.
11. Sprawdź, czy wynik prowadzi do `stitch_help_premium`.
12. Wybierz projekt `hat`.
13. Sprawdź, czy wynik przypomina o próbce i mierzeniu.
14. Wybierz projekt `blanket`.
15. Sprawdź, czy wynik mówi o długim projekcie, partii włóczki i notatkach.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `crochet-soft-craft`.
2. Sprawdź, czy ekran wyniku pokazuje kroki i listę potrzebnych rzeczy.
3. Sprawdź, czy wynik można dodać do ulubionych.
4. Sprawdź, czy wynik zapisuje się w historii.
5. Sprawdź, czy pełna karta projektu pokazuje się po reklamie.

## Testy offline MVP

1. Sprawdź, czy aplikacja nie wymaga AI w MVP.
2. Sprawdź, czy wyniki są oparte na statycznych danych JSON.
3. Sprawdź, czy eksport projektu może być traktowany jako funkcja po reklamie.
4. Sprawdź, czy licznik i notatki mogą działać lokalnie.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy eksport projektu jest opisany jako funkcja po reklamie.
4. Sprawdź, czy dodatkowe wzory mogą być promowane po reklamie.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją,
- MVP nie wymaga AI i może działać offline.
