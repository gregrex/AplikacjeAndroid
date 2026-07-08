# Silikon Fuga Fix — manualne testy MVP

## Zakres

Testy obejmują proste prace przy silikonie i fugach: czyszczenie, ocenę pleśni, wymianę starego silikonu, ocenę pęknięć, odspojenia oraz zatrzymanie pracy przy przecieku albo wysokim ryzyku.

## Testy funkcjonalne

1. Uruchom aplikację i wybierz projekt `Silikon Fuga Fix`.
2. Sprawdź, czy projekt jest widoczny w katalogu aplikacji.
3. Wybierz kategorię `dirty_grout`.
4. Ustaw: problem `dirty_grout`, miejsce `floor`, zabrudzenie `medium`, materiał `ceramic`, ryzyko `low`.
5. Sprawdź, czy wynik premium prowadzi do `dirty_grout_clean_premium`.
6. Wybierz kategorię `old_silicone`.
7. Sprawdź, czy wynik mówi o usunięciu starego silikonu, czystym i suchym podłożu.
8. Wybierz kategorię `mold`.
9. Sprawdź, czy wynik rozróżnia powierzchniowy nalot od pleśni w strukturze silikonu.
10. Wybierz kategorię `crack`.
11. Sprawdź, czy wynik każe ocenić stabilność i przerwać przy wodzie.
12. Wybierz kategorię `detached`.
13. Sprawdź, czy wynik nie pozwala doklejać odspojenia po wierzchu.
14. Wybierz kategorię `suspected_leak`.
15. Sprawdź, czy wynik prowadzi do `leak_stop_premium` i każe przerwać DIY.

## Testy wysokiego ryzyka

1. W dowolnej kategorii ustaw `risk = high`.
2. Sprawdź, czy wynik prowadzi do `high_risk_stop_premium`.
3. Sprawdź, czy wynik nie daje instrukcji naprawy instalacji ani ukrywania wilgoci.
4. Sprawdź, czy wynik sugeruje zdjęcia, opis objawów i kontakt z fachowcem.

## Testy językowe

1. Ustaw język PL i sprawdź tytuły wyników.
2. Ustaw język EN i sprawdź, czy `resultId` są zgodne z PL.
3. Ustaw język UK i sprawdź, czy `resultId` są zgodne z PL.
4. Sprawdź, czy każdy wynik ma `title`, `summary` i `steps`.

## Testy UX i runtime

1. Sprawdź, czy ładuje się theme `sealant-fix-slate-blue`.
2. Sprawdź, czy ekran wyniku pokazuje listę kroków.
3. Sprawdź, czy karta ostrzeżeń jest widoczna przy ryzyku i przecieku.
4. Sprawdź, czy wynik można dodać do ulubionych.
5. Sprawdź, czy wynik zapisuje się w historii.
6. Sprawdź, czy pełna procedura pokazuje się po reklamie.

## Testy bezpieczeństwa

1. Sprawdź, czy aplikacja nie prowadzi przez naprawę instalacji wodnej.
2. Sprawdź, czy aplikacja nie zachęca do zakrywania aktywnego przecieku silikonem.
3. Sprawdź, czy aplikacja ostrzega przed pracą na mokrym podłożu.
4. Sprawdź, czy aplikacja ostrzega przed mieszaniem chemii.
5. Sprawdź, czy przy kamieniu naturalnym jest ostrożność z agresywną chemią.

## Testy monetyzacji

1. Sprawdź wariant darmowy dla każdej reguły.
2. Sprawdź wariant premium po reklamie dla każdej reguły.
3. Sprawdź, czy wariant premium ma pełniejszą procedurę lub checklistę.
4. Sprawdź, czy wariant bezpieczeństwa jest dostępny bez reklamy przy przecieku i wysokim ryzyku.

## Kryterium zaliczenia

Projekt można oznaczyć jako MVP-ready, gdy:

- dane przechodzą walidator,
- PL/EN/UK mają te same `resultId`,
- runtime pack istnieje w `wwwroot`,
- projekt jest widoczny w katalogu,
- theme ładuje się dla wybranego projektu,
- store listing i raport MVP istnieją,
- scenariusze ryzykowne nie prowadzą użytkownika przez niebezpieczne prace.
