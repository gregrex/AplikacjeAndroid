# Pokój Makeover — manualne testy MVP

## Zakres

Testy obejmują metamorfozę pokoju bez remontu, bez analizy zdjęcia, z formularza: typ pokoju, styl, budżet, problem i czas.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Pokój Makeover`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `minimal`.
4. Odpowiedz: typ pokoju `bedroom`, styl `minimal`, budżet `low`, problem `clutter`, czas `weekend`.
5. Sprawdź, czy wynik premium prowadzi do `minimal_declutter_premium`.
6. Wybierz kategorię `cozy` i styl `cozy`.
7. Sprawdź, czy wynik mówi o świetle, tekstyliach i ciepłym akcencie.
8. Wybierz kategorię `colorful` i styl `colorful`.
9. Sprawdź, czy wynik używa jednego koloru akcentowego.
10. Wybierz kategorię `gaming` i styl `gaming`.
11. Sprawdź, czy wynik nie zasłania wentylacji sprzętu i porządkuje kable.
12. Wybierz kategorię `study_work` i styl `study_work`.
13. Sprawdź, czy wynik porządkuje biurko i światło.
14. Wybierz kategorię `small_room` i styl `small_room`.
15. Sprawdź, czy wynik mówi o pomiarach i przechowywaniu pionowym.

## Testy budżetu

1. Ustaw budżet `zero`.
2. Sprawdź, czy wynik `zero_budget_*` istnieje i jest dostępny w danych.
3. Sprawdź, czy wariant 0 zł nie proponuje zakupów jako pierwszego kroku.
4. Sprawdź, czy aplikacja zachęca do użycia rzeczy już posiadanych.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `room-makeover-indigo-warm`.
2. Sprawdź, czy ekran wyniku pokazuje plan zmian krok po kroku.
3. Sprawdź, czy wynik można dodać do ulubionych.
4. Sprawdź, czy wynik zapisuje się w historii.
5. Sprawdź, czy pełny plan pokazuje się po reklamie.

## Testy zasad MVP

1. Sprawdź, czy aplikacja nie próbuje analizować zdjęcia.
2. Sprawdź, czy wynik jasno bazuje na odpowiedziach z formularza.
3. Sprawdź, czy aplikacja nie proponuje remontu jako pierwszego kroku.
4. Sprawdź, czy przy małym pokoju wynik nie każe kupować dużych mebli bez pomiaru.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy wariant premium ma listę zakupów lub DIY.
4. Sprawdź, czy wariant budżetowy po reklamie może być promowany osobno.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją.
