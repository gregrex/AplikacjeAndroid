# DomFix — manualne testy MVP

## Zakres

Testy obejmują bezpieczne drobne naprawy domowe, regułę zatrzymania przy wysokim ryzyku, runtime, parytet językowy, marketing i monetyzację.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `DomFix`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `door`.
4. Odpowiedz: problem `squeaky_door`, miejsce `room`, narzędzia `basic`, ryzyko `low`, woda `none`, materiał `metal`.
5. Sprawdź, czy wynik premium prowadzi do `squeaky_door_premium`.
6. Wybierz kategorię `hinge` i problem `loose_hinge`.
7. Sprawdź, czy wynik nie zachęca do pracy przy ciężkich drzwiach wejściowych.
8. Wybierz kategorię `drain` i problem `clogged_drain`.
9. Sprawdź, czy wynik ostrzega przed mieszaniem środków chemicznych.
10. Wybierz kategorię `silicone` i problem `silicone`.
11. Sprawdź, czy wynik mówi o suchym, odtłuszczonym podłożu.
12. Wybierz kategorię `grout` i problem `grout`.
13. Sprawdź, czy wynik odróżnia brudną fugę od uszkodzonej fugi.
14. Wybierz kategorię `zipper` i problem `zipper`.
15. Sprawdź, czy wynik mówi, aby nie szarpać suwaka.

## Test bezpieczeństwa wysokiego ryzyka

1. Wybierz dowolną kategorię, np. `drain`.
2. Ustaw `risk` na `high`.
3. Sprawdź, czy aplikacja zwraca `high_risk_stop_premium` po odblokowaniu premium.
4. Sprawdź, czy wynik nakazuje przerwanie pracy i wezwanie fachowca.
5. Sprawdź, czy wynik nie prowadzi użytkownika przez elektrykę, gaz, konstrukcję, aktywny wyciek ani pracę przy ciśnieniu.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `domfix-teal-safe`.
2. Sprawdź, czy ekran wyniku używa checklisty technicznej lub instrukcyjnej.
3. Sprawdź, czy karta ostrzeżeń jest widoczna przy wynikach z `warnings`.
4. Sprawdź, czy wynik można dodać do ulubionych.
5. Sprawdź, czy wynik zapisuje się w historii.
6. Sprawdź, czy pełna instrukcja pokazuje się po reklamie.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy wariant premium ma listę potrzebnych rzeczy lub pełniejsze kroki.
4. Sprawdź, czy lista zakupów może być pokazana po reklamie.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- reguła wysokiego ryzyka działa przed regułami napraw,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją.
