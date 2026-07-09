# Plan dopracowania projektów AppFactory

## Cel

Celem jest podniesienie jakości repozytorium `AplikacjeAndroid` z poziomu kolekcji MVP do stabilnej platformy mikroaplikacji mobilnych.

Priorytetem nie jest dodawanie kolejnych projektów, tylko:

- ujednolicenie jakości danych,
- automatyczna walidacja wszystkich projektów,
- lepszy silnik reguł,
- spójny UX,
- przygotowanie pod publikację i monetyzację.

## Zasada pracy

Każdy projekt powinien być wyłącznie paczką danych i motywu:

- `app.json`,
- `categories.json`,
- `questions.json`,
- `rules.json`,
- `results.pl.json`,
- `results.en.json`,
- `results.uk.json`,
- `theme.json`,
- `marketing/store-listing.pl.md`,
- `tests/manual-tests.md`.

Logika aplikacji, walidacja, historia, ulubione, reklamy i eksport powinny pozostać wspólne dla całej platformy.

## Etap 1 — automatyczna kontrola jakości danych

### Kroki

1. Dodać globalny test wszystkich projektów.
2. Sprawdzić, czy każdy projekt z katalogu ma katalog w `projects`.
3. Sprawdzić, czy każdy projekt ma komplet danych źródłowych.
4. Sprawdzić, czy każdy projekt ma runtime mirror w `wwwroot`.
5. Sprawdzić, czy PL/EN/UK mają te same `resultId`.
6. Sprawdzić, czy każdy wynik ma `title`, `summary`, `steps`.
7. Sprawdzić, czy każda reguła wskazuje istniejące wyniki.
8. Sprawdzić, czy każda reguła wskazuje istniejącą kategorię albo wildcard `*`.
9. Sprawdzić, czy `theme.json` istnieje w źródle i runtime.
10. Sprawdzić, czy istnieją pliki manual tests i marketing.

### Efekt

Po tym etapie repo powinno mieć jeden test, który szybko pokazuje, czy nowy albo zmieniony projekt nie psuje platformy.

## Etap 2 — rozszerzenie walidatora

### Kroki

1. Rozszerzyć `DataPackValidationService` o walidację pustych pól.
2. Dodać walidację `app.json`.
3. Dodać walidację `theme.json`.
4. Dodać walidację spójności runtime z `projects`.
5. Dodać walidację minimalnej liczby kategorii, pytań i reguł.
6. Dodać raport błędów z nazwą projektu i pliku.

### Efekt

Błędy danych będą wykrywane automatycznie, zanim trafią do builda aplikacji.

## Etap 3 — raport jakości

### Kroki

1. Dodać dokument `docs/quality/QUALITY_STATUS.md`.
2. Wypisać projekty, które są MVP-ready.
3. Wypisać brakujące elementy, jeśli testy coś znajdą.
4. Dodać status ostatniej kontroli.
5. Dodać kolejne rekomendacje.

### Efekt

Repo będzie miało czytelny punkt kontroli jakości.

## Etap 4 — generator runtime

### Kroki

1. Dodać skrypt, który kopiuje `projects/<id>/data` do `wwwroot/projects/<id>`.
2. Skrypt powinien kopiować też `theme.json`.
3. Skrypt powinien wykrywać brakujące pliki.
4. Skrypt powinien generować raport różnic.
5. Docelowo skrypt może być częścią CI.

### Efekt

Nie trzeba będzie ręcznie dublować danych w runtime.

## Etap 5 — generator testów projektu

### Kroki

1. Dodać szablon testu danych.
2. Dodać szablon testu parytetu językowego.
3. Dodać skrypt generujący testy dla nowego projektu.
4. Z czasem zastąpić wiele ręcznych testów jednym testem parametrycznym.

### Efekt

Nowe projekty będą miały testy tworzone automatycznie.

## Etap 6 — ulepszenie silnika reguł

### Kroki

1. Dodać wynik alternatywny, nie tylko najlepszy.
2. Dodać pole `reason` albo `why` dla reguł.
3. Dodać listę dopasowanych warunków.
4. Dodać poziom pewności rekomendacji.
5. Dodać fallback bardziej czytelny dla użytkownika.

### Efekt

Aplikacje będą mniej „sztywne”, a użytkownik zrozumie, dlaczego dostał dany wynik.

## Etap 7 — wspólny UX

### Kroki

1. Ujednolicić ekran wyniku.
2. Dodać sekcję `Dlaczego taki wynik`.
3. Dodać sekcję `Co możesz zrobić teraz`.
4. Dodać wariant `pełna checklista po reklamie`.
5. Ujednolicić historię i ulubione.
6. Dodać eksport tekstu.

### Efekt

Wszystkie mikroaplikacje będą wyglądały i działały jak jedna spójna platforma.

## Etap 8 — publikacja i sklep

### Kroki

1. Uzupełnić listingi EN i UK.
2. Przygotować politykę prywatności.
3. Przygotować FAQ.
4. Przygotować screenshoty.
5. Przygotować ikony i grafiki promocyjne.
6. Przygotować warianty buildów dla osobnych aplikacji.

### Efekt

Repo będzie gotowe do publikacji w Google Play.

## Kolejność wykonania

1. Globalne testy jakości.
2. Raport jakości.
3. Rozszerzenie walidatora.
4. Generator runtime.
5. Generator testów.
6. Silnik reguł.
7. UX.
8. Publikacja.

## Status

Ten dokument rozpoczyna etap jakościowy. Pierwszy techniczny krok to dodanie globalnych testów dla wszystkich projektów.
