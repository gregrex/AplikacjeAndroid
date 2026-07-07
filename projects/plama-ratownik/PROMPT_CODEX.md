# Prompt dla Codexa — Plama Ratownik

## Rola

Jesteś agentem kodującym pracującym nad projektem `Plama Ratownik` w repozytorium `AplikacjeAndroid`.

## Cel

Przygotuj dane i konfigurację aplikacji `Plama Ratownik`, która działa na wspólnym silniku .NET MAUI Blazor Hybrid.

Nie buduj osobnego backendu. Nie trenuj AI. Nie integruj zewnętrznego LLM.

## Funkcje MVP

1. Użytkownik wybiera typ plamy.
2. Użytkownik wybiera materiał.
3. Użytkownik określa, czy plama jest świeża.
4. Aplikacja dobiera regułę.
5. Aplikacja pokazuje wynik podstawowy.
6. Użytkownik może obejrzeć reklamę rewarded.
7. Aplikacja pokazuje pełną instrukcję.
8. Wynik można zapisać w historii i ulubionych.

## Dane do przygotowania

Rozbuduj pliki w `projects/plama-ratownik/data/`:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `i18n/pl.json`,
- później `results.en.json`, `results.uk.json`, `i18n/en.json`, `i18n/uk.json`.

## Minimalny zakres danych

Typy plam:

- kawa,
- herbata,
- tłuszcz,
- czerwone wino,
- krew,
- trawa,
- błoto,
- długopis,
- kosmetyki,
- rdza.

Materiały:

- bawełna,
- jeans,
- poliester,
- wełna,
- jedwab,
- len.

## Ostrzeżenia

Aplikacja musi ostrzegać:

- aby testować środek w niewidocznym miejscu,
- aby nie używać gorącej wody przy krwi,
- aby nie suszyć ubrania, dopóki plama nie zniknie,
- aby przy delikatnych tkaninach działać ostrożnie.

## Definicja gotowości

Projekt jest gotowy, gdy shared-engine potrafi załadować dane i przejść cały scenariusz od kategorii do pełnego wyniku.
