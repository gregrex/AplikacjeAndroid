# Outfit Coach — manualne testy MVP

## Zakres

Testy obejmują dobór zestawu do okazji, pogody i stylu, parytet językowy, runtime, theme, bezpieczeństwo treści i monetyzację.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Outfit Coach`.
2. Sprawdź, czy projekt jest widoczny w katalogu.
3. Wybierz okazję `interview`.
4. Ustaw pogodę `mild`, styl `smart`, górę `shirt`, dół `chinos`, buty `loafers`.
5. Sprawdź, czy wynik prowadzi do `interview_smart_premium`.
6. Wybierz okazję `meeting` i styl `smart`.
7. Sprawdź, czy wynik prowadzi do `meeting_smart_premium`.
8. Wybierz okazję `school`.
9. Sprawdź, czy wynik opisuje wygodę, warstwę i buty na długi dzień.
10. Wybierz okazję `party`.
11. Sprawdź, czy wynik proponuje jeden mocniejszy akcent, a nie wiele naraz.
12. Wybierz okazję `walk` i pogodę `rain`.
13. Sprawdź, czy wynik mówi o wygodnych i niesliskich butach oraz warstwie pogodowej.
14. Wybierz okazję `family`.
15. Sprawdź, czy wynik jest schludny i spokojny.

## Testy językowe

1. Ustaw język PL i sprawdź wyniki.
2. Ustaw język EN i sprawdź, czy zestaw `resultId` jest taki sam jak w PL.
3. Ustaw język UK i sprawdź, czy zestaw `resultId` jest taki sam jak w PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `outfit-soft-blue`.
2. Sprawdź, czy karta wyniku jest czytelna i zawiera checklistę.
3. Sprawdź dodawanie wyniku do ulubionych.
4. Sprawdź zapis w historii.
5. Sprawdź eksport lub kopiowanie tekstu wyniku.

## Testy bezpieczeństwa treści

1. Sprawdź, czy aplikacja nie ocenia ciała, wagi, atrakcyjności ani osoby.
2. Sprawdź, czy komunikaty oceniają wyłącznie dopasowanie ubrań do okazji i pogody.
3. Sprawdź, czy wyniki nie używają zawstydzającego języka.
4. Sprawdź, czy wyniki zawierają praktyczne ostrzeżenia: pogoda, wygoda, buty, warstwy.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy wynik premium zawiera pełniejszą checklistę niż darmowy.
4. Sprawdź, czy można przygotować listę zakupów lub elementów do uzupełnienia po reklamie.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją.
