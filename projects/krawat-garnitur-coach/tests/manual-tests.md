# Krawat Garnitur Coach — manualne testy MVP

## Zakres

Testy obejmują dobór formalnego zestawu: garnitur, koszulę, krawat lub muchę, buty i pasek do okazji.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Krawat Garnitur Coach`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz okazję `interview`.
4. Ustaw: garnitur `navy`, koszula `white`, dodatek `tie`, buty `black`.
5. Sprawdź, czy wynik premium prowadzi do `interview_safe_premium`.
6. Wybierz okazję `business` i sprawdź `business_classic_premium`.
7. Wybierz okazję `exam` i sprawdź `exam_simple_premium`.
8. Wybierz okazję `family_event` i sprawdź `family_event_balanced_premium`.
9. Wybierz okazję `evening` i sprawdź `evening_elegant_premium`.
10. Ustaw `suit_color = black` i sprawdź, czy aplikacja ostrzega, że czarny garnitur nie jest uniwersalny.
11. Ustaw `neckwear = none` i sprawdź, czy aplikacja informuje, że brak krawata obniża formalność.

## Testy zasad

1. Sprawdź, czy aplikacja nie ocenia osoby, sylwetki, twarzy ani atrakcyjności.
2. Sprawdź, czy wyniki mówią wyłącznie o zgodności elementów stroju z okazją.
3. Sprawdź, czy aplikacja nie generuje komentarzy personalnych.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `formal-classic-coach`.
2. Sprawdź, czy ekran wyniku pokazuje bezpieczny zestaw, czego unikać i checklistę.
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
- aplikacja ocenia tylko elementy stroju i okazję, nie osobę.
