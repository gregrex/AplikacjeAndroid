# Fryzury Proste — manualne testy MVP

## Zakres

Testy obejmują proste, neutralne instrukcje uczesań, parytet językowy, runtime, theme, monetyzację i bezpieczeństwo treści.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Fryzury Proste`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `quick`.
4. Odpowiedz: typ włosów `straight`, czas `five`, okazja `work`, akcesoria `elastic`, efekt `natural`.
5. Sprawdź, czy wynik premium prowadzi do `quick_five_premium`.
6. Wybierz kategorię `sport`.
7. Ustaw okazję `sport` i sprawdź, czy wynik prowadzi do `sport_hold_premium`.
8. Wybierz kategorię `everyday` i efekt `natural`.
9. Sprawdź, czy wynik opisuje neutralne, codzienne uczesanie.
10. Wybierz kategorię `elegant` i efekt `neat`.
11. Sprawdź, czy wynik pokazuje spokojne eleganckie upięcie.
12. Ustaw typ włosów `curly`.
13. Sprawdź, czy wynik nie każe agresywnie rozczesywać suchych loków.
14. Ustaw akcesoria `clip`.
15. Sprawdź, czy wynik pokazuje wariant z klamrą.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `hair-simple-violet`.
2. Sprawdź, czy ekran wyniku pokazuje instrukcję krok po kroku.
3. Sprawdź, czy karta ostrzeżeń jest widoczna przy wynikach z `warnings`.
4. Sprawdź, czy wynik można dodać do ulubionych.
5. Sprawdź, czy wynik zapisuje się w historii.
6. Sprawdź, czy pełny wariant pokazuje się po reklamie.

## Testy bezpieczeństwa treści

1. Sprawdź, czy aplikacja nie ocenia twarzy, atrakcyjności ani osoby.
2. Sprawdź, czy wynik opisuje techniczne kroki uczesania.
3. Sprawdź, czy wynik nie zawiera zawstydzających ocen wyglądu.
4. Sprawdź, czy instrukcje nie zachęcają do bolesnego spinania lub szarpania włosów.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy wariant premium ma więcej kroków lub akcesoriów.
4. Sprawdź, czy paczki tematyczne mogą być promowane po reklamie.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją.
