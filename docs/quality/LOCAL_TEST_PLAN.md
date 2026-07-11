# Plan testów lokalnych AppFactory

## Cel

Celem pierwszej lokalnej sesji jest ustalenie rzeczywistego stanu repozytorium przed oznaczeniem aplikacji jako `production-ready`.

Plan obejmuje:

- kompilację i testy automatyczne,
- SQLite i migrację danych,
- lokalne logi JSONL, logcat i eksport diagnostyki,
- build Android Debug i Release,
- smoke test wspólnego UI,
- 100 scenariuszy produkcyjnych,
- Local AI obrazu i dźwięku,
- testy offline, restartu i uprawnień,
- rejestrację defektów oraz kryteria wydania.

## 1. Przygotowanie środowiska

Wymagane:

- Windows 11,
- .NET SDK 9,
- workload .NET MAUI/Android,
- Android SDK i zgodny JDK,
- PowerShell 7,
- emulator Android albo telefon z włączonym debugowaniem USB.

Kontrola:

```powershell
dotnet --info
dotnet workload list
adb devices
```

Jeżeli brakuje workloadu:

```powershell
dotnet workload restore src/AppFactory.Mobile/AppFactory.Mobile.csproj
```

## 2. Czysty punkt startowy

```powershell
git status
git pull
dotnet nuget locals all --clear
```

Przed pierwszym testem urządzenia:

- odinstaluj starszą wersję tylko wtedy, gdy nie testujesz migracji,
- pozostaw starszą wersję i jej dane, jeżeli testujesz migrację `Preferences -> SQLite`,
- zapisz commit SHA,
- zapisz model telefonu, wersję Androida i orientację ekranu,
- wyczyść wcześniejszy logcat, aby ograniczyć szum:

```powershell
adb logcat -c
```

## 3. Szybka sesja automatyczna

Najpierw uruchom gotowy skrypt:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1
```

Pełniejsza wersja z workload restore i buildem Release:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Runner wykonuje:

- kontrolę SDK i workloadów,
- restore projektów,
- wszystkie testy automatyczne,
- osobny przebieg testów SQLite,
- osobny przebieg testów lokalnego logowania,
- Android Debug build,
- opcjonalny Android Release build,
- raport jakości,
- `adb devices`,
- snapshot ostatnich wpisów logcat.

Oczekiwany wynik:

- restore bez błędów,
- wszystkie testy `PASS`,
- Android Debug build bez błędów,
- Android Release build bez błędów,
- raport testów w `artifacts/local-test/<timestamp>`.

Jeżeli runner zatrzyma się na błędzie, napraw najpierw pierwszy zgłoszony problem. Nie rozpoczynaj pełnej rundy manualnej na niezielonym buildzie.

## 4. Testy automatyczne według obszaru

### 4.1 Wszystkie testy

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release
```

### 4.2 SQLite

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter "FullyQualifiedName~AppDatabaseTests|FullyQualifiedName~LocalDatabaseProductionTests"
```

Sprawdza:

- inicjalizację schematu,
- health check,
- sortowanie i deduplikację historii,
- limit 100 wpisów,
- deduplikację i usuwanie ulubionych,
- kontrakt migracji starszych danych.

### 4.3 Reguły biznesowe

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter FullyQualifiedName~AllProjectRuleReachabilityTests
```

### 4.4 Scenariusze i akcje UI

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter FullyQualifiedName~ProjectProductionScenariosTests
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter FullyQualifiedName~ScenarioImplementationAuditTests
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter FullyQualifiedName~UiUxProductionTests
```

### 4.5 Local AI

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter "FullyQualifiedName~ImageAnalysisServiceTests|FullyQualifiedName~AudioAnalysisServiceTests|FullyQualifiedName~LocalAiModelStoreTests|FullyQualifiedName~LocalAiInputTensorFactoryTests|FullyQualifiedName~AiSuggestionWorkflowTests"
```

### 4.6 Logowanie i diagnostyka

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter "FullyQualifiedName~LocalLogStoreTests|FullyQualifiedName~DiagnosticsProductionTests"
```

Sprawdza:

- format JSONL,
- maskowanie e-maili, tokenów, haseł i sekretów,
- rotację plików,
- retencję i limit liczby plików,
- odczyt końca logu,
- czyszczenie logów,
- obecność ekranu diagnostyki,
- ręczny eksport ZIP,
- brak automatycznego klienta wysyłającego logi,
- obecność collectora ADB i logcat.

## 5. Build Android

### Debug

```powershell
dotnet build src/AppFactory.Mobile/AppFactory.Mobile.csproj -f net9.0-android -c Debug
```

### Release

```powershell
dotnet build src/AppFactory.Mobile/AppFactory.Mobile.csproj -f net9.0-android -c Release
```

Kryterium PASS:

- brak błędów kompilacji,
- brak błędów linkera i natywnych bibliotek SQLite/ONNX,
- APK/AAB powstaje w katalogu wynikowym,
- aplikacja uruchamia się bez crasha na ekranie startowym,
- ekran `Ustawienia -> Logi i diagnostyka` otwiera się.

## 6. Weryfikacja logów przed testami funkcjonalnymi

Wykonaj tę procedurę przed smoke testem i przed 100 scenariuszami.

1. Uruchom aplikację Debug.
2. Przejdź do `Ustawienia -> Logi i diagnostyka`.
3. Zapisz widoczny identyfikator sesji w trackerze.
4. Kliknij `Zapisz znacznik testowy`.
5. Potwierdź, że w podglądzie pojawia się `LOCAL_TEST_MARKER`.
6. Kliknij `Zapisz testowy wyjątek`.
7. Potwierdź wpis `DIAGNOSTICS_TEST_EXCEPTION` zawierający typ wyjątku i stack trace.
8. Kliknij `Eksportuj i udostępnij ZIP`.
9. Otwórz ZIP i sprawdź:
   - `diagnostics-manifest.json`,
   - minimum jeden plik `logs/*.jsonl`,
   - wersję i build aplikacji,
   - producenta, model i wersję Androida,
   - health check SQLite,
   - zgodność identyfikatora sesji.
10. Sprawdź, że eksport działa bez aktywnego internetu.
11. Dla buildu Debug uruchom:

```powershell
pwsh ./tools/quality/pull-android-diagnostics.ps1 -CreateZip
```

12. Sprawdź katalog:

```text
artifacts/device-diagnostics/<timestamp>
```

Powinien zawierać:

- `device-manifest.json`,
- `logcat.txt`,
- `package-dumpsys.txt`,
- `adb-devices.txt`,
- `app-logs/*.jsonl`, jeśli `adb run-as` jest dostępne,
- `summary.md`.

Jeżeli eksport aplikacyjny lub collector ADB nie działa, oznacz diagnostykę jako `FAIL`. Nie rozpoczynaj długiej rundy testów bez przynajmniej jednego sprawnego kanału pobierania logów.

## 7. Smoke test wspólnego UI — około 15 minut

Wykonaj przed 100 scenariuszami:

1. Na ekranie diagnostyki kliknij `Zapisz znacznik testowy`.
2. Uruchom katalog.
3. Sprawdź wyszukiwanie i filtr typu aplikacji.
4. Otwórz dowolne trzy projekty o różnych profilach UI.
5. Sprawdź topbar, dolną nawigację i przycisk systemowy Wstecz.
6. Przejdź jeden quiz do wyniku.
7. Odblokuj pełny wynik.
8. Dodaj wynik do ulubionych.
9. Otwórz historię i ulubione.
10. Zamknij proces aplikacji i uruchom go ponownie.
11. Sprawdź, czy historia i ulubione nadal istnieją.
12. Otwórz diagnostykę i sprawdź wpisy:
    - zmianę projektu,
    - decyzję silnika reguł,
    - zapis historii,
    - zapis ulubionego,
    - inicjalizację lub migrację SQLite.
13. Eksportuj ZIP po zakończeniu smoke testu.

Jeżeli smoke test nie przechodzi, nie rozpoczynaj pełnych 100 scenariuszy.

## 8. Procedura logowania dla każdego scenariusza

Przed rozpoczęciem serii scenariuszy projektu:

1. Otwórz diagnostykę.
2. Zapisz identyfikator sesji.
3. Kliknij `Zapisz znacznik testowy`.
4. Zapisz w trackerze czas rozpoczęcia serii.

Dla każdego `FAIL` lub `BLOCKED`:

1. Nie zamykaj od razu aplikacji, chyba że scenariusz tego wymaga.
2. Zapisz dokładny czas z dokładnością co najmniej do minuty.
3. Zapisz projekt i numer scenariusza.
4. Zrób screenshot albo nagranie ekranu.
5. Otwórz diagnostykę, jeżeli aplikacja nadal działa.
6. Eksportuj ZIP.
7. Uruchom collector ADB:

```powershell
pwsh ./tools/quality/pull-android-diagnostics.ps1 -CreateZip
```

8. Do defektu dołącz:
   - ZIP z aplikacji,
   - katalog lub ZIP collectora ADB,
   - `artifacts/local-test/<timestamp>`,
   - screenshot lub nagranie,
   - commit SHA,
   - numer buildu,
   - model urządzenia i Android,
   - oczekiwany i rzeczywisty wynik,
   - informację, czy problem powtarza się po restarcie.

Nie loguj ręcznie pełnych nazw plików użytkownika, danych osobowych, treści zdjęcia ani surowego audio. Do identyfikacji wystarczą typ MIME, rozmiar, projekt, wynik walidacji i czas operacji.

## 9. Priorytetowa kolejność projektów

### Grupa A — bezpieczeństwo i diagnostyka

Testuj jako pierwsze:

- `zmywarka-diagnosta`,
- `silikon-fuga-fix`,
- `domfix`,
- `kolek-dobieracz`,
- `chleb-zakwas-coach`,
- `plama-ratownik`,
- `pakowanie-paczek`,
- `router-wifi-diagnosta`.

Każdy błąd prowadzący do ryzykownej instrukcji ma priorytet krytyczny.

### Grupa B — Local AI

- `plama-ratownik`,
- `pokoj-makeover`,
- `rysunek-coach`,
- `outfit-coach`,
- `fryzury-proste`,
- `barber-translator`,
- `zmywarka-diagnosta`,
- `silikon-fuga-fix`,
- `pies-trener-7dni`,
- `kot-bawi-sie`.

### Grupa C — pozostałe aplikacje

Testuj po przejściu grup A i B.

## 10. Wykonanie 100 scenariuszy

Źródło scenariuszy:

```text
projects/<projectId>/tests/production-scenarios.md
```

Tracker:

```text
docs/quality/SCENARIO_EXECUTION_TRACKER.md
```

Dla każdego scenariusza zapisz:

- `PASS`, `FAIL`, `BLOCKED` albo `NOT_RUN`,
- urządzenie i wersję Androida,
- datę i dokładny czas błędu,
- numer buildu i commit,
- identyfikator sesji logów,
- numer defektu,
- ścieżkę do paczki diagnostycznej,
- screenshot albo nagranie dla `FAIL` i `BLOCKED`.

Scenariusz jest `PASS`, gdy:

- wszystkie kroki można wykonać,
- akcje istnieją i reagują,
- wynik odpowiada oczekiwaniu,
- nie występuje crash,
- nie ma utraty danych,
- ostrzeżenia bezpieczeństwa są widoczne tam, gdzie są wymagane,
- logi nie zawierają nieoczekiwanych błędów związanych ze scenariuszem.

## 11. Test lokalnej bazy SQLite

### Nowa instalacja

1. Wyczyść dane aplikacji.
2. Uruchom aplikację.
3. Otwórz diagnostykę i potwierdź schemat bazy.
4. Wygeneruj trzy różne wyniki.
5. Dodaj dwa wyniki do ulubionych.
6. Zamknij proces aplikacji.
7. Uruchom ponownie.
8. Sprawdź historię i ulubione.
9. Usuń jedno ulubione.
10. Wyczyść historię.
11. Uruchom aplikację ponownie i sprawdź stan.
12. Sprawdź w logach zapis, odczyt i czyszczenie danych.

### Migracja ze starszej wersji

1. Na starszym buildzie zapisz historię i ulubione.
2. Zainstaluj nowy build bez usuwania danych.
3. Otwórz historię i ulubione.
4. Sprawdź możliwość ponownego otwarcia każdego wyniku.
5. Otwórz diagnostykę i wyszukaj wpisy migracji z liczbą rekordów.
6. Uruchom aplikację ponownie.
7. Potwierdź, że dane nie zostały zduplikowane.
8. Eksportuj paczkę diagnostyczną.

Kryterium PASS:

- brak utraty danych,
- brak duplikatów,
- poprawna kolejność historii,
- poprawne usuwanie i czyszczenie,
- brak crasha przy uszkodzonym starszym JSON,
- log zawiera wynik migracji bez danych osobowych.

## 12. Local AI

Przed testem sprawdź:

- skonfigurowany `DownloadUrl`,
- poprawny `Sha256`,
- dostępność modelu,
- wystarczającą ilość miejsca na urządzeniu.

Testuj:

1. Brak modelu — analiza powinna być zablokowana czytelnym komunikatem i wpisem warning.
2. Błędny SHA256 — plik tymczasowy powinien zostać odrzucony i usunięty.
3. Poprawny model — analiza powinna uruchomić ONNX.
4. Zdjęcie lub nagranie nieobsługiwanego typu — wejście powinno zostać odrzucone.
5. Plik zbyt duży albo nagranie zbyt długie — wejście powinno zostać odrzucone.
6. Sugestia AI — nie może zmienić quizu bez kliknięcia `Użyj tej sugestii`.
7. Tryb offline po pobraniu — analiza powinna działać bez sieci.
8. Log powinien zawierać:
   - rozpoczęcie pobierania,
   - host bez pełnego URL i bez query string,
   - rozmiar modelu,
   - wynik SHA256,
   - czas pobierania,
   - rozpoczęcie i zakończenie analizy,
   - czas inferencji lub analizy,
   - liczbę sugestii.

Jeżeli docelowe modele nie są jeszcze skonfigurowane, scenariusze AI oznacz `BLOCKED`, a nie `PASS`.

## 13. Testy niefunkcjonalne

Sprawdź minimum:

- start bez internetu,
- utratę internetu podczas pobierania modelu,
- małą ilość miejsca,
- odmowę dostępu do plików,
- anulowanie FilePicker,
- obrót ekranu,
- przejście aplikacji do tła i powrót,
- systemowy przycisk Wstecz,
- bardzo długie tłumaczenia,
- powiększony rozmiar tekstu systemowego,
- tryb ciemny systemu,
- szybkie wielokrotne kliknięcia przycisków,
- urządzenie o niższej wydajności,
- rotację logów przy małym limicie w teście automatycznym,
- eksport diagnostyki bez sieci,
- zachowanie przy niedostępnym miejscu zapisu logów.

## 14. Klasyfikacja defektów

### Krytyczny

- crash aplikacji,
- utrata lub uszkodzenie danych,
- niebezpieczna porada,
- brak możliwości uruchomienia aplikacji,
- niesprawny build Release,
- brak jakiegokolwiek sposobu odzyskania diagnostyki po crashu.

### Wysoki

- scenariusz główny nie może zostać ukończony,
- wynik wskazuje złą regułę,
- historia lub ulubione nie działają,
- AI omija ręczne potwierdzenie,
- podstawowa nawigacja jest uszkodzona,
- logi zapisują sekret lub dane osobowe bez maskowania.

### Średni

- błąd alternatywnej ścieżki,
- problem z tłumaczeniem,
- nieprawidłowy stan pusty,
- problem z układem utrudniający obsługę,
- niekompletny manifest diagnostyczny.

### Niski

- kosmetyka, odstęp, literówka lub drobna niespójność bez wpływu na wykonanie scenariusza,
- mało czytelny komunikat diagnostyczny bez utraty danych.

## 15. Kryterium zakończenia testów

Status `production-ready` można nadać dopiero gdy:

- wszystkie testy automatyczne są zielone,
- Debug i Release Android build przechodzą,
- testy logowania i maskowania przechodzą,
- `LOCAL_TEST_MARKER` i testowy wyjątek są widoczne,
- eksport ZIP działa offline,
- collector ADB pobiera logcat i logi aplikacji dla buildu Debug,
- smoke test przechodzi,
- 100/100 scenariuszy ma `PASS`,
- brak otwartych defektów krytycznych i wysokich,
- migracja SQLite jest potwierdzona,
- Local AI ma realne modele i przechodzi testy albo jest wyłączone w buildzie wydaniowym,
- wykonano regresję po ostatniej poprawce.

## 16. Pierwsza sesja testowa — zalecany zakres

Na pierwszą sesję wykonaj:

1. `run-local-test-plan.ps1`,
2. test ekranu diagnostyki,
3. zapis `LOCAL_TEST_MARKER`,
4. zapis kontrolowanego `DIAGNOSTICS_TEST_EXCEPTION`,
5. eksport ZIP,
6. `pull-android-diagnostics.ps1 -CreateZip`,
7. smoke test,
8. pełny test SQLite,
9. pięć scenariuszy `zmywarka-diagnosta`,
10. pięć scenariuszy `silikon-fuga-fix`,
11. pięć scenariuszy `szydelko-pomocnik`,
12. zapis defektów z czasem, sesją i paczką logów,
13. decyzję, czy repo nadaje się do dalszej pełnej rundy.
