# Rysunek Coach — manualne testy MVP

## Zakres

Testy obejmują lekcje rysowania krok po kroku, neutralne tematy, brak naruszania praw autorskich, runtime, parytet językowy, theme, monetyzację i gotowość katalogową.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Rysunek Coach`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `animals`.
4. Odpowiedz: temat `animal`, poziom `easy`, czas `short`, narzędzie `pencil`, styl `simple`.
5. Sprawdź, czy wynik premium prowadzi do `animal_easy_premium`.
6. Wybierz kategorię `vehicles` i temat `vehicle`.
7. Sprawdź, czy wynik nie używa logo, marek ani konkretnych modeli.
8. Wybierz kategorię `plants` i temat `plant`.
9. Sprawdź, czy wynik prowadzi przez doniczkę, łodygi i liście.
10. Wybierz kategorię `simple_people` i temat `person`.
11. Sprawdź, czy wynik opisuje neutralną postać bez kopiowania bohaterów.
12. Wybierz kategorię `robots` i temat `robot`.
13. Sprawdź, czy wynik prowadzi przez robota z figur geometrycznych.
14. Wybierz kategorię `houses` i temat `house`.
15. Sprawdź, czy wynik prowadzi przez domek i proste tło.
16. Wybierz kategorię `monsters` i temat `monster`.
17. Sprawdź, czy wynik mówi o własnym, autorskim potworku.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `drawing-coach-blue-paper`.
2. Sprawdź, czy ekran wyniku pokazuje instrukcję krok po kroku.
3. Sprawdź, czy karta ostrzeżeń jest widoczna przy wynikach z `warnings`.
4. Sprawdź, czy wynik można dodać do ulubionych.
5. Sprawdź, czy wynik zapisuje się w historii.
6. Sprawdź, czy pełna lekcja pokazuje się po reklamie.

## Testy bezpieczeństwa treści i praw autorskich

1. Sprawdź, czy aplikacja nie używa nazw znanych postaci, bajek, gier, filmów ani marek.
2. Sprawdź, czy pojazdy nie mają logo ani nazw modeli.
3. Sprawdź, czy roboty, potworki i postacie są opisane jako własne, neutralne projekty.
4. Sprawdź, czy wyniki uczą rysowania przez figury i kroki, a nie przez kopiowanie cudzych projektów.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy wariant premium ma pełniejszą kartę ćwiczeń.
4. Sprawdź, czy eksport karty ćwiczeń może być promowany po reklamie.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- wyniki nie zawierają nazw chronionych postaci ani marek,
- store listing i raport MVP istnieją.
