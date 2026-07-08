# Chleb Zakwas Coach — manualne testy MVP

## Zakres

Testy obejmują prowadzenie zakwasu i podstawowe problemy przy pieczeniu chleba: zakwas nie rośnie, ciasto nie wyrasta, zakalec, blada skórka, pękanie chleba, podejrzany zapach i harmonogram karmienia.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Chleb Zakwas Coach`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `starter_not_rising`.
4. Ustaw: problem `starter_not_rising`, etap `starter`, temperatura `cold`, czas `ok`, zapach `acidic`.
5. Sprawdź, czy wynik premium prowadzi do `starter_not_rising_premium`.
6. Wybierz kategorię `dough_not_rising`.
7. Sprawdź, czy wynik mówi o czasie, temperaturze i aktywnym zakwasie.
8. Wybierz kategorię `dense_crumb`.
9. Sprawdź, czy wynik mówi o wystudzeniu chleba i fermentacji.
10. Wybierz kategorię `pale_crust`.
11. Sprawdź, czy wynik mówi o temperaturze, czasie pieczenia i ostrożności z parą.
12. Wybierz kategorię `cracking`.
13. Sprawdź, czy wynik mówi o nacięciu, końcowym wyrastaniu i wilgotności powierzchni.
14. Wybierz kategorię `feeding_schedule`.
15. Sprawdź, czy wynik pokazuje prosty harmonogram karmienia.

## Testy bezpieczeństwa żywności

1. Wybierz kategorię `odd_smell`.
2. Ustaw `smell = mold`.
3. Sprawdź, czy wynik prowadzi do `discard_mold_premium`.
4. Sprawdź, czy aplikacja każe wyrzucić cały zakwas, a nie zdejmować tylko wierzch.
5. Ustaw `smell = rotten`.
6. Sprawdź, czy wynik prowadzi do `discard_rotten_premium`.
7. Sprawdź, czy aplikacja nie zachęca do karmienia zepsutego zakwasu dalej.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `sourdough-warm-bread`.
2. Sprawdź, czy ekran wyniku pokazuje pierwszy krok, plan i ostrzeżenia.
3. Sprawdź, czy wynik można dodać do ulubionych.
4. Sprawdź, czy wynik zapisuje się w historii.
5. Sprawdź, czy pełny plan pokazuje się po reklamie.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy harmonogram karmienia jest opisany jako wariant po reklamie.
4. Sprawdź, czy scenariusze bezpieczeństwa żywności są jasne również w wariancie darmowym.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją,
- pleśń i zapach zepsucia zawsze prowadzą do wyrzucenia zakwasu.
