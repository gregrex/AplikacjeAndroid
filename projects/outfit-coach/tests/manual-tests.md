# Outfit Coach — manualne testy MVP

## Zakres

Testy obejmują dobór stroju do okazji, pogody i stylu, parytet językowy, runtime, bezpieczeństwo treści, monetyzację i gotowość marketingową.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Outfit Coach`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `interview`.
4. Odpowiedz: okazja `interview`, pogoda `mild`, styl `smart`, góra `shirt`, dół `chinos`, buty `loafers`.
5. Sprawdź, czy wynik premium prowadzi do `interview_smart_premium`.
6. Wybierz kategorię `meeting`.
7. Odpowiedz: okazja `meeting`, styl `smart`, pogoda `mild`.
8. Sprawdź, czy wynik prowadzi do `meeting_smart_premium`.
9. Wybierz kategorię `school`.
10. Sprawdź, czy wynik zawiera wygodę, plecak/torbę i warstwę do zdjęcia.
11. Wybierz kategorię `party`.
12. Sprawdź, czy wynik mówi o jednym mocnym akcencie i wygodnych butach.
13. Wybierz kategorię `walk` oraz pogodę `rain`.
14. Sprawdź, czy wynik mówi o butach, warstwie pogodowej i deszczu.
15. Wybierz pogodę `cold` i sprawdź wariant warstwowy.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `outfit-soft-blue`.
2. Sprawdź czy ekran wyniku jest czytelny na jasnym tle.
3. Sprawdź, czy wynik można dodać do ulubionych.
4. Sprawdź, czy wynik zapisuje się w historii.
5. Sprawdź, czy tekst listy kontrolnej można skopiować lub wyeksportować.

## Testy bezpieczeństwa treści

1. Sprawdź, czy aplikacja nie ocenia ciała, wagi, atrakcyjności ani osoby.
2. Sprawdź, czy wynik ocenia wyłącznie dopasowanie stroju do okazji, pogody i wygody.
3. Sprawdź, czy tekst nie używa zwrotów zawstydzających użytkownika.
4. Sprawdź, czy wyniki zachęcają do komfortu, czystości, pogody i praktyczności.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy wynik premium ma pełniejszą instrukcję niż darmowy.
4. Sprawdź, czy lista zakupów/checklista może być pokazana po reklamie.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją.
