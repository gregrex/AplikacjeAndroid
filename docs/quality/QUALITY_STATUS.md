# Status jakości projektów AppFactory

## Status produkcyjny

Aktualny status repo: **production-ready candidate**.

Kod zawiera komplet danych, scenariuszy, profili UI/UX, lokalną bazę SQLite, lokalne logi diagnostyczne i automatyczne gate'y implementacji. Finalny status `production-ready` wymaga zielonego CI oraz wykonania scenariuszy na Androidzie.

## Zakres katalogu

Katalog obejmuje 20 projektów.

## UI/UX wszystkich aplikacji

Każdy projekt ma dedykowany profil:

- ikonę,
- badge doświadczenia,
- własny hero title i opis,
- nagłówki kategorii, quizu i wyniku,
- typ widoku wyniku,
- etykiety akcji premium i zapisu,
- konfigurację safety, kopiowania lub narzędzi specjalnych.

Wspólny design system obejmuje:

- sticky topbar,
- dolną nawigację,
- safe-area Android,
- responsywne siatki,
- gradientowe hero,
- większe cele dotykowe,
- progress quizu,
- kafle odpowiedzi,
- stany puste,
- karty bezpieczeństwa,
- warianty wyniku zależne od projektu,
- `prefers-reduced-motion`.

Raport:

```text
docs/quality/UI_UX_AUDIT.md
```

## Lokalna baza danych

Dodano osobny projekt:

```text
src/AppFactory.Persistence/AppFactory.Persistence.csproj
```

Pakiet:

```text
sqlite-net-pcl 1.9.172
```

SQLite przechowuje:

- historię wyników,
- ulubione wyniki,
- wersję schematu.

Historia jest deduplikowana, sortowana i ograniczona do 100 rekordów. Ulubione są deduplikowane oraz można je usuwać pojedynczo lub czyścić w całości.

`HistoryService` i `FavoritesService` wykonują migrację wcześniejszych list JSON z `Preferences`.

W ustawieniach aplikacji widoczny jest health check bazy:

- stan,
- wersja schematu,
- liczba wpisów historii,
- liczba ulubionych.

Testy:

```text
tests/AppFactory.Mobile.Tests/AppDatabaseTests.cs
tests/AppFactory.Mobile.Tests/LocalDatabaseProductionTests.cs
```

Dokumentacja:

```text
docs/quality/LOCAL_DATABASE.md
```

## Lokalne logi i diagnostyka

Dodano provider `Microsoft.Extensions.Logging` zapisujący lokalne pliki JSONL.

Polityka:

- Debug od poziomu `Debug`,
- Release od poziomu `Information`,
- retencja 7 dni,
- maksymalnie 12 plików,
- maksymalnie 2 MB na plik,
- identyfikator sesji w każdym wpisie,
- automatyczne maskowanie e-maili, tokenów, haseł, sekretów i Bearer,
- brak automatycznej wysyłki.

Rejestrowane są między innymi:

- start aplikacji i wersja buildu,
- nieobsłużone wyjątki .NET i Android,
- migracje SQLite,
- zapis i czyszczenie historii oraz ulubionych,
- wybór obrazu lub audio,
- tworzenie paczki diagnostycznej.

Ekran:

```text
Ustawienia -> Logi i diagnostyka
```

Pozwala:

- zobaczyć ostatnie 100 wpisów,
- zapisać `LOCAL_TEST_MARKER`,
- sprawdzić stan SQLite,
- wyczyścić logi,
- ręcznie udostępnić ZIP z manifestem urządzenia i logami.

Awaryjne pobranie z buildu Debug:

```powershell
pwsh ./tools/quality/pull-android-diagnostics.ps1 -CreateZip
```

Skrypt zbiera prywatne logi przez `adb run-as`, logcat, dumpsys package i metadane urządzenia do:

```text
artifacts/device-diagnostics/<timestamp>
```

Testy:

```text
tests/AppFactory.Mobile.Tests/LocalLogStoreTests.cs
tests/AppFactory.Mobile.Tests/DiagnosticsProductionTests.cs
```

Dokumentacja:

```text
docs/quality/LOCAL_LOGGING.md
```

## Scenariusze produkcyjne

Dla każdego projektu istnieje:

```text
projects/<projectId>/tests/production-scenarios.md
```

Łączne pokrycie:

- **20 projektów**,
- **5 scenariuszy na projekt**,
- **100 scenariuszy produkcyjnych**.

Testy:

```text
tests/AppFactory.Mobile.Tests/ProjectProductionScenariosTests.cs
tests/AppFactory.Mobile.Tests/AllProjectRuleReachabilityTests.cs
tests/AppFactory.Mobile.Tests/ScenarioImplementationAuditTests.cs
```

## Audyt akcji i logiki biznesowej

Każdy scenariusz jest mapowany na wymagane capabilities:

- katalog i motyw,
- quiz i silnik reguł,
- wynik free/premium,
- ulubione i historia,
- języki PL/EN/UK,
- fallback,
- alternatywy,
- kopiowanie,
- Local AI image/audio,
- trwałość danych,
- safety,
- narzędzia projektu.

Test sprawdza dowody implementacji w faktycznych stronach, usługach, danych i politykach projektu.

## Local AI on device

Runtime:

```text
Microsoft.ML.OnnxRuntime
```

Warstwa obejmuje downloader, SHA256, lokalny model store, tensor input, provider obrazu, provider audio, FilePicker oraz ręczne zatwierdzanie sugestii w quizie.

Aktualny kontrakt wejścia:

```text
input: float32[1,1,1,256]
```

Modele `local-vision-v1` i `local-audio-v1` wymagają skonfigurowania adresu pobierania, SHA256 i docelowych etykiet klas.

## Plan testów lokalnych

Dodano:

```text
docs/quality/LOCAL_TEST_PLAN.md
tools/quality/run-local-test-plan.ps1
```

Zalecane pierwsze uruchomienie:

```powershell
pwsh ./tools/quality/run-local-test-plan.ps1 -RestoreWorkloads -IncludeReleaseBuild -WriteReport
```

Runner zapisuje logi, pliki TRX, osobny przebieg testów logowania, logcat i podsumowanie w:

```text
artifacts/local-test/<timestamp>
```

Tracker wykonania został rozszerzony o:

- dane urządzenia i buildu,
- identyfikator sesji logów,
- testy logowania i eksportu,
- automatyczne gate'y,
- testy SQLite i migracji,
- macierz 100 scenariuszy,
- regresję końcową,
- tabelę defektów z dowodami diagnostycznymi.

## Weryfikacja wymagana przed publikacją

1. Uruchomić lokalny runner testów.
2. Naprawić pierwszy błąd kompilacji albo testu.
3. Wykonać smoke test na Androidzie.
4. Potwierdzić `LOCAL_TEST_MARKER`, eksport ZIP i pobranie przez ADB.
5. Wykonać test nowej bazy i migracji `Preferences -> SQLite`.
6. Wykonać 100 scenariuszy na emulatorze lub urządzeniu.
7. Zapisać PASS/FAIL wraz z czasem i identyfikatorem sesji logów.
8. Sprawdzić FilePicker, klawiaturę, safe-area, systemowy back i długie tłumaczenia.
9. Naprawić wszystkie błędy krytyczne i wysokie.
10. Skonfigurować docelowe modele ONNX albo wyłączyć funkcje AI w buildzie publikacyjnym do czasu gotowości modeli.

## Uwagi

Nie uruchamiałem lokalnie `dotnet test`, ponieważ zmiany wykonuję przez GitHub connector. Kompilacja logowania, SQLite, Android build, eksport ZIP i pobieranie logów przez ADB wymagają teraz potwierdzenia podczas Twojej lokalnej sesji testowej.
