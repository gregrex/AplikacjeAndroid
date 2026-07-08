# Bukietownik — manualne testy MVP

## Zakres

Testy obejmują dobór bukietu do okazji, kolorów, kwiatów i stylu, parytet językowy, runtime, theme, monetyzację i gotowość marketingową.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Bukietownik`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `birthday`.
4. Odpowiedz: okazja `birthday`, kolory `colorful`, kwiaty `mixed`, styl `simple`, rozmiar `medium`.
5. Sprawdź, czy wynik premium prowadzi do `birthday_colorful_premium`.
6. Wybierz kategorię `thanks`.
7. Sprawdź, czy wynik opisuje delikatny bukiet z podziękowaniem.
8. Wybierz kategorię `family`.
9. Sprawdź, czy wynik mówi o niższym bukiecie na stół.
10. Wybierz kategorię `home_decor`.
11. Sprawdź, czy wynik mówi o zieleni, wazonie i świeżej wodzie.
12. Wybierz kategorię `just_because`.
13. Sprawdź, czy wynik opisuje luźny, naturalny bukiet.
14. Ustaw kwiaty `roses` i styl `romantic`.
15. Sprawdź, czy wariant różany ostrzega przed niedopasowaniem romantycznego tonu do formalnej okazji.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `bouquet-pink-green`.
2. Sprawdź, czy ekran wyniku pokazuje instrukcję układania bukietu.
3. Sprawdź, czy karta ostrzeżeń jest widoczna przy wynikach z `warnings`.
4. Sprawdź, czy wynik można dodać do ulubionych.
5. Sprawdź, czy wynik zapisuje się w historii.
6. Sprawdź, czy wariant premium pokazuje się po reklamie.

## Testy praktyczne

1. Sprawdź, czy wynik zawiera kolejność układania.
2. Sprawdź, czy wynik zawiera potrzebne rzeczy.
3. Sprawdź, czy wynik zawiera pielęgnację bukietu tam, gdzie ma to sens.
4. Sprawdź, czy wynik unika zbyt skomplikowanych florystycznych technik.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy wariant premium ma więcej kroków lub opis do social media.
4. Sprawdź, czy paczki stylów mogą być promowane po reklamie.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją.
