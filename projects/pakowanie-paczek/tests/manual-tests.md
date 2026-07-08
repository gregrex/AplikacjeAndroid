# Pakowanie Paczek — manualne testy MVP

## Zakres

Testy obejmują dobór zabezpieczenia przedmiotu do wysyłki: typ przedmiotu, kruchość, rozmiar, transport, wartość, wariant darmowy, wariant premium i checklistę przed nadaniem.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Pakowanie Paczek`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `books`.
4. Ustaw: przedmiot `books`, kruchość `medium`, rozmiar `medium`, transport `courier`, wartość `medium`.
5. Sprawdź, czy wynik premium prowadzi do `books_corners_premium`.
6. Wybierz kategorię `clothes`.
7. Sprawdź, czy wynik mówi o suchym ubraniu, woreczku i ochronie przed wilgocią.
8. Wybierz kategorię `glass`.
9. Sprawdź, czy wynik mówi o oddzielnym owinięciu i braku luzu w kartonie.
10. Wybierz kategorię `electronics`.
11. Sprawdź, czy wynik mówi o ochronie ekranu, kablach i ubezpieczeniu przy wysokiej wartości.
12. Wybierz kategorię `cosmetics`.
13. Sprawdź, czy wynik mówi o woreczkach i zabezpieczeniu zamknięć.
14. Wybierz kategorię `shoes`.
15. Sprawdź, czy wynik mówi o zachowaniu kształtu i zewnętrznym kartonie.
16. Wybierz kategorię `small_appliance`.
17. Sprawdź, czy wynik mówi o buforze z każdej strony kartonu.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `parcel-safe`.
2. Sprawdź, czy ekran wyniku pokazuje checklistę pakowania.
3. Sprawdź, czy wynik można dodać do ulubionych.
4. Sprawdź, czy wynik zapisuje się w historii.
5. Sprawdź, czy pełna checklista pokazuje się po reklamie.

## Testy zasad bezpieczeństwa

1. Sprawdź, czy aplikacja nie gwarantuje bezpieczeństwa przesyłki.
2. Sprawdź, czy przy wartościowych lub delikatnych rzeczach pojawia się sugestia ubezpieczenia.
3. Sprawdź, czy aplikacja przypomina o regulaminie przewoźnika.
4. Sprawdź, czy wynik nie obiecuje, że paczka na pewno dotrze bez uszkodzeń.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy wariant premium ma pełniejszą checklistę.
4. Sprawdź, czy wariant ekonomiczny i bezpieczny może być promowany po reklamie.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją.
