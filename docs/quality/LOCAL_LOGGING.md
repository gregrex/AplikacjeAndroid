# Lokalne logi i diagnostyka AppFactory

## Cel

Logi mają umożliwić odtworzenie błędu podczas lokalnych testów bez zewnętrznej telemetrii i bez automatycznego wysyłania danych.

## Źródła diagnostyki

### 1. Logi aplikacyjne JSONL

Katalog na Androidzie:

```text
<FileSystem.AppDataDirectory>/logs
```

Typowa ścieżka prywatna:

```text
/data/user/0/pl.gbcom.appfactory/files/logs
```

Każdy wiersz pliku `.jsonl` jest osobnym obiektem JSON zawierającym:

- czas UTC,
- identyfikator sesji,
- poziom logu,
- kategorię,
- `EventId`,
- wiadomość,
- typ, wiadomość i stack trace wyjątku.

### 2. Android logcat

Logcat zawiera informacje z runtime Androida, MAUI, WebView, ONNX, SQLite oraz błędy natywne.

### 3. Testy i buildy lokalne

Runner zapisuje:

- stdout i stderr każdego kroku,
- pliki TRX,
- build Debug i Release,
- snapshot logcat,
- podsumowanie Markdown.

Katalog:

```text
artifacts/local-test/<timestamp>
```

## Retencja i rotacja

Domyślna polityka:

- Debug: poziom `Debug` i wyższy,
- Release: poziom `Information` i wyższy,
- retencja: 7 dni,
- maksymalnie 12 plików,
- maksymalnie 2 MB na plik,
- format: UTF-8 JSONL.

Logowanie nigdy nie może zatrzymać aplikacji. Błąd zapisu logu jest ignorowany, aby nie zastąpił pierwotnego wyjątku.

## Prywatność

Przed zapisem automatycznie maskowane są:

- adresy e-mail,
- hasła,
- tokeny access/refresh,
- nagłówki Authorization,
- API keys,
- sekrety,
- podpisy i tokeny w query string,
- wartości Bearer.

Logi:

- pozostają lokalnie,
- nie korzystają z endpointu telemetrycznego,
- nie są wysyłane automatycznie,
- wymagają ręcznej akcji eksportu.

Przed dołączeniem paczki do defektu nadal należy ją przejrzeć.

## Automatycznie logowane zdarzenia

- start aplikacji i wersja buildu,
- nieobsłużone wyjątki .NET,
- nieobserwowane wyjątki zadań,
- nieobsłużone wyjątki Android runtime,
- inicjalizacja i migracja SQLite,
- zapis, odczyt i czyszczenie historii,
- zapis, usunięcie i czyszczenie ulubionych,
- otwarcie i anulowanie FilePicker,
- skopiowanie pliku obrazu lub audio do cache,
- tworzenie i udostępnianie paczki diagnostycznej,
- ręczny `LOCAL_TEST_MARKER`.

Kolejne serwisy powinny używać standardowego `ILogger<T>` zamiast bezpośredniego zapisu do plików.

## Eksport z aplikacji

Ścieżka:

```text
Ustawienia -> Logi i diagnostyka
```

Dostępne akcje:

- podgląd ostatnich 100 wpisów,
- zapis znacznika `LOCAL_TEST_MARKER`,
- odświeżenie statusu,
- wyczyszczenie logów,
- przygotowanie i ręczne udostępnienie ZIP.

Paczka ZIP zawiera:

```text
diagnostics-manifest.json
logs/*.jsonl
```

Manifest zawiera:

- wersję i build aplikacji,
- package name,
- producenta i model urządzenia,
- wersję Androida,
- identyfikator sesji,
- health check SQLite,
- statystyki logów,
- informację o prywatności.

## Pobranie przez ADB

Dla buildu Debug:

```powershell
pwsh ./tools/quality/pull-android-diagnostics.ps1 -CreateZip
```

Skrypt zbiera:

- prywatne logi aplikacji przez `adb run-as`,
- ostatnie wpisy logcat,
- `dumpsys package`,
- listę urządzeń ADB,
- metadane urządzenia,
- podsumowanie Markdown.

Katalog:

```text
artifacts/device-diagnostics/<timestamp>
```

Gdy `run-as` nie działa:

1. sprawdź, czy zainstalowany jest build Debug,
2. sprawdź package name,
3. uruchom aplikację przynajmniej raz,
4. użyj eksportu ZIP z ekranu diagnostyki.

## Minimalna procedura dla każdego defektu

1. Otwórz ekran diagnostyki.
2. Kliknij `Zapisz znacznik testowy`.
3. Wykonaj kroki prowadzące do błędu.
4. Zapisz dokładny czas wystąpienia błędu.
5. Eksportuj paczkę ZIP albo uruchom skrypt ADB.
6. Dołącz screenshot lub nagranie ekranu.
7. Zapisz commit SHA, build, urządzenie i wersję Androida.
8. Opisz wynik oczekiwany i rzeczywisty.

## Testy automatyczne

```text
tests/AppFactory.Mobile.Tests/LocalLogStoreTests.cs
tests/AppFactory.Mobile.Tests/DiagnosticsProductionTests.cs
```

Zakres:

- format JSONL,
- maskowanie danych,
- rotacja plików,
- limit liczby plików,
- odczyt końca logu,
- czyszczenie,
- obecność UI eksportu,
- brak klienta HTTP w eksporcie,
- zbieranie logcat przez runner.

Uruchomienie:

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter "FullyQualifiedName~LocalLogStoreTests|FullyQualifiedName~DiagnosticsProductionTests"
```
