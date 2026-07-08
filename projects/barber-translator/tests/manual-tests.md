# Barber Translator — manualne testy MVP

## Zakres

Testy obejmują dobór instrukcji fryzury, parytet językowy, runtime, bezpieczeństwo treści, monetyzację i gotowość marketingową.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Barber Translator`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `fade`.
4. Odpowiedz: góra `short`, boki `clipper_short`, przejście `mid_fade`, grzywka `short`, tekstura `straight`, styl `modern`.
5. Sprawdź, czy wynik premium prowadzi do `short_fade_premium`.
6. Wybierz kategorię `formal`.
7. Odpowiedz: styl `formal`, przejście `no_fade`, góra `medium`.
8. Sprawdź, czy wynik prowadzi do klasycznej instrukcji formalnej.
9. Wybierz kategorię `curly`.
10. Odpowiedz: tekstura `curly`, góra `longer`, boki `keep_length`.
11. Sprawdź, czy wynik ostrzega przed zbyt mocnym skracaniem loków.
12. Wybierz kategorię `natural` i boki `keep_length`.
13. Sprawdź, czy wynik mówi o odświeżeniu fryzury bez dużej zmiany.
14. Wybierz grzywkę `textured`.
15. Sprawdź, czy wynik zawiera teksturowaną grzywkę i kontrolę długości z przodu.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `barber-dark`.
2. Sprawdź, czy ekran wyniku jest czytelny w ciemnym motywie.
3. Sprawdź, czy wynik można dodać do ulubionych.
4. Sprawdź, czy wynik zapisuje się w historii.
5. Sprawdź, czy tekst instrukcji można skopiować lub wykorzystać jako eksport.

## Testy bezpieczeństwa treści

1. Sprawdź, czy aplikacja nie ocenia wyglądu użytkownika.
2. Sprawdź, czy tekst skupia się na preferencjach fryzury.
3. Sprawdź, czy aplikacja nie obiecuje efektu zależnego od urody, twarzy lub atrakcyjności.
4. Sprawdź, czy instrukcje zachęcają do potwierdzenia długości przed mocnym skróceniem.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy wynik premium ma pełniejszą instrukcję niż darmowy.
4. Sprawdź, czy scenariusz może być użyty do generowania obrazu lub karty do pokazania barberowi.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją.
