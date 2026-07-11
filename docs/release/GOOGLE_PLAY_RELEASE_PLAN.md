# Google Play release plan — AppFactory Pomocniki 1.0

## Decyzja produktu

Publikowany jest jeden produkt:

- nazwa: `AppFactory Pomocniki`,
- package ID: `pl.gbcom.appfactory`,
- wersja: `1.0.0`,
- version code: `1`,
- typ: jedna aplikacja katalogowa z 20 pomocnikami,
- kategoria Google Play: `Tools`,
- grupa docelowa: osoby dorosłe,
- monetyzacja 1.0: brak reklam i płatności,
- konto: niewymagane,
- Local AI: wyłączone w domyślnym buildzie Release do czasu dostarczenia zweryfikowanych modeli.

## Co jest przygotowane w repo

### Produkt i bezpieczeństwo wydania

- konfiguracja AAB Release,
- target Android API 35,
- finalna nazwa i numer wersji,
- pełne wyniki bez mockowanych reklam,
- release feature flags,
- lokalne SQLite,
- lokalne logi z ręcznym eksportem,
- polityka prywatności i pomoc w aplikacji,
- publiczna strona pod GitHub Pages.

### Marka i Google Play

- launcher icon i splash,
- źródło ikony Google Play,
- źródło feature graphic,
- generator PNG 512×512 i 1024×500,
- listingi dla 24 języków urzędowych UE oraz ukraińskiego,
- exporter metadanych,
- plan sześciu screenshotów,
- instrukcja marketingowa.

### Release engineering

- skrypt tworzenia upload keystore,
- skrypt budowania podpisanego AAB,
- workflow GitHub Actions dla AAB,
- checklista Play Console,
- gate testów gotowości sklepowej.

## Etapy wykonania

### Etap 1 — zielony kod

1. Uruchom `run-local-test-plan.ps1`.
2. Napraw wszystkie błędy kompilacji i testów.
3. Potwierdź build Debug i Release.
4. Potwierdź działanie SQLite, logów, historii i ulubionych.
5. Wykonaj 100 scenariuszy produkcyjnych.

Kryterium przejścia: wszystkie testy zielone, brak błędów krytycznych i wysokich.

### Etap 2 — artefakty sklepu

1. Uruchom `generate-play-graphics.ps1`.
2. Uruchom aplikację na telefonie 1080×1920.
3. Wykonaj sześć screenshotów według `SCREENSHOT_PLAN.md`.
4. Uruchom `export-play-metadata.ps1`.
5. Zweryfikuj wszystkie obrazy i teksty.

Kryterium przejścia: komplet plików w `artifacts/google-play`.

### Etap 3 — podpis i AAB

1. Utwórz upload keystore.
2. Zapisz klucz i hasła poza repozytorium.
3. Uruchom `build-play-aab.ps1`.
4. Prześlij AAB do Internal testing.
5. Sprawdź raport pre-launch i App Bundle Explorer.

Kryterium przejścia: podpisany AAB zaakceptowany przez Play Console.

### Etap 4 — Play Console

1. Utwórz aplikację z package ID `pl.gbcom.appfactory`.
2. Wstaw listing i grafiki.
3. Uzupełnij App content według przygotowanych deklaracji.
4. Ustaw politykę prywatności i stronę wsparcia.
5. Włącz Play App Signing.
6. Utwórz Internal testing.
7. Po smoke teście rozpocznij Closed testing, jeżeli wymaga tego konto.

### Etap 5 — test zamknięty i produkcja

1. Zbierz testerów.
2. Prowadź test przez wymagany okres bez przerw.
3. Napraw zgłoszone błędy.
4. Wydaj build kandydujący z nowym version code.
5. Złóż wniosek o dostęp produkcyjny, jeżeli dotyczy.
6. Opublikuj etapowo, zaczynając od ograniczonego procentu użytkowników.

## Elementy, których repo nie może wykonać automatycznie

- utworzenie lub opłacenie konta Google Play Developer,
- weryfikacja tożsamości i urządzenia właściciela konta,
- akceptacja umów Google,
- utworzenie i bezpieczne przechowanie prywatnego klucza,
- ręczne odpowiedzi prawne składane przez właściciela konta,
- wykonanie prawdziwych screenshotów uruchomionej aplikacji,
- udział realnych testerów,
- kliknięcie publikacji w Play Console.

Te kroki są oznaczone w checkliście jako `OWNER_ACTION`.

## Kryterium statusu production-ready

Status można nadać, gdy:

- AAB Release jest podpisany i zaakceptowany,
- target API spełnia aktualne wymaganie Google Play,
- publiczna polityka prywatności działa,
- listing i grafiki są kompletne,
- Data Safety odpowiada zachowaniu binarki,
- 100/100 scenariuszy ma PASS,
- raport pre-launch nie zawiera blokerów,
- brak otwartych defektów krytycznych i wysokich,
- wymagany test zamknięty został ukończony.
