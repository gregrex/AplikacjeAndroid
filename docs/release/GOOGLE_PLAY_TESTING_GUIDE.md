# Testy Google Play — AppFactory Pomocniki

## Cel

Testy przez Google Play sprawdzają nie tylko sam kod, ale również:

- podpis i dostarczenie AAB,
- instalację z Play Store,
- zachowanie po aktualizacji,
- konfigurację urządzeń,
- pre-launch report,
- realne scenariusze użytkowników.

## 1. Internal testing

### Konfiguracja

1. Utwórz aplikację `AppFactory Pomocniki` w Play Console.
2. Włącz Play App Signing.
3. Utwórz track `Internal testing`.
4. Dodaj małą listę 3–10 zaufanych kont Google.
5. Wgraj podpisany AAB.
6. Dodaj release notes PL/EN/UK.
7. Opublikuj release wewnętrzny.
8. Skopiuj link opt-in i sprawdź go w trybie prywatnym przeglądarki.

### Minimalny smoke test

Każdy tester wykonuje:

1. instalację wyłącznie z linku Google Play,
2. pierwsze uruchomienie,
3. otwarcie trzech pomocników,
4. ukończenie jednego quizu,
5. zapis wyniku do ulubionych,
6. ponowne otwarcie z historii,
7. restart procesu,
8. zmianę języka,
9. eksport diagnostyki,
10. aktualizację z następnego buildu bez czyszczenia danych.

### Kryterium wyjścia

- instalacja i aktualizacja działają,
- brak crashy oraz ANR,
- SQLite zachowuje dane po aktualizacji,
- pełny wynik jest dostępny bez reklamy,
- Local AI nie jest widoczne,
- publiczne linki prywatności i pomocy działają,
- pre-launch report nie zawiera blokerów.

## 2. Closed testing

Closed testing rozpoczyna się dopiero po zielonym Internal testing.

### Grupa

Rekomendowane minimum organizacyjne:

- 15–25 aktywnych testerów,
- różne marki telefonów,
- co najmniej trzy wersje Androida,
- przynajmniej jedno urządzenie o niższej wydajności,
- testerzy przypisani do konkretnych pomocników.

Jeżeli Play Console narzuca minimalną liczbę testerów i czas testu dla danego konta, stosuj wartość widoczną bezpośrednio w konsoli. Nie zakładaj z góry, że wymaganie konta jest identyczne jak w innych kontach.

### Podział scenariuszy

- Grupa A: dom i bezpieczeństwo,
- Grupa B: styl i hobby,
- Grupa C: historia, ulubione, języki i dostępność,
- Grupa D: aktualizacja, SQLite i diagnostyka.

Każdy tester otrzymuje:

- link opt-in,
- wersję builda,
- 2–5 scenariuszy,
- formularz zgłoszenia,
- instrukcję eksportu logów,
- informację, że aplikacja nie zastępuje specjalisty.

### Zasady

- nie proś o pozytywne recenzje,
- nie oferuj korzyści za ocenę,
- proś o opis problemu, nie tylko ocenę,
- zapisuj urządzenie, Android, czas i build,
- dla każdego FAIL wymagaj kroków odtworzenia,
- logi są dobrowolne i wymagają świadomego eksportu.

## 3. Pre-launch report

Sprawdź:

- crashy,
- ANR,
- problemy dostępności,
- błędy renderowania,
- nieobsługiwane urządzenia,
- zrzuty ekranów z crawlera,
- uprawnienia i zachowanie przy pierwszym uruchomieniu.

Każdy crash albo ANR jest blokerem publikacji do czasu wyjaśnienia.

## 4. Aktualizacje testowe

Pierwszy build:

```text
1.0.0 / version code 1
```

Kolejny testowy AAB musi mieć wyższy version code, np.:

```text
1.0.1 / version code 2
```

Nie wolno ponownie wysłać AAB z użytym wcześniej version code.

Test aktualizacji:

1. zainstaluj build 1,
2. zapisz historię i ulubione,
3. opublikuj build 2 na tym samym tracku,
4. wykonaj aktualizację przez Google Play,
5. sprawdź dane, ustawienia, logi i działanie quizu,
6. nie czyść danych pomiędzy wersjami.

## 5. Raport testera

Wymagane pola:

- tester,
- data i czas,
- build/version code,
- urządzenie,
- Android,
- pomocnik i scenariusz,
- wynik PASS/FAIL/BLOCKED,
- oczekiwane zachowanie,
- rzeczywiste zachowanie,
- kroki odtworzenia,
- powtarzalność po restarcie,
- screenshot/nagranie,
- opcjonalna paczka diagnostyczna.

## 6. Decyzja o produkcji

Produkcja jest dopuszczalna, gdy:

- wymagania dostępu produkcyjnego konta są spełnione,
- Internal i wymagany Closed testing są zakończone,
- 100 scenariuszy ma PASS,
- brak crashy i ANR w pre-launch report,
- brak otwartych defektów krytycznych i wysokich,
- polityka, Data Safety i listing odpowiadają finalnej binarce,
- finalny AAB ma właściwy version code i podpis.
