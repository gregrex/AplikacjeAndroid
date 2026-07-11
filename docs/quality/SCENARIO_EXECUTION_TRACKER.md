# Tracker wykonania scenariuszy produkcyjnych

## Dane sesji

Uzupełnij przed rozpoczęciem:

- Data:
- Tester:
- Commit SHA:
- Konfiguracja: `Debug` / `Release`
- Urządzenie lub emulator:
- Wersja Androida:
- Rozdzielczość i orientacja:
- Wersja aplikacji:
- Identyfikator sesji logów:
- Modele Local AI: `READY` / `NOT_CONFIGURED` / `BLOCKED`
- Katalog logów automatycznych: `artifacts/local-test/<timestamp>`
- Katalog diagnostyki urządzenia: `artifacts/device-diagnostics/<timestamp>`
- Nazwa paczki ZIP z aplikacji:

## Zasady

Dozwolone statusy:

- `NOT_RUN` — scenariusz nie został wykonany,
- `PASS` — wynik zgodny z oczekiwaniem,
- `FAIL` — wynik niezgodny; wymagany numer defektu,
- `BLOCKED` — scenariusza nie można wykonać z powodu zależności, konfiguracji lub modelu.

Każdy status `FAIL` lub `BLOCKED` musi mieć:

- opis,
- numer defektu albo blokera,
- dokładny czas zdarzenia,
- identyfikator sesji logów,
- paczkę diagnostyczną albo logcat,
- screenshot lub nagranie,
- informację, czy problem powtarza się po restarcie.

## Automatyczne gate'y

| Obszar | Status | Log/uwagi |
| --- | --- | --- |
| Restore | NOT_RUN | |
| Wszystkie testy .NET | NOT_RUN | |
| Testy SQLite | NOT_RUN | |
| Testy lokalnego logowania | NOT_RUN | |
| Testy osiągalności reguł | NOT_RUN | |
| Testy scenariuszy i akcji | NOT_RUN | |
| Android Debug build | NOT_RUN | |
| Android Release build | NOT_RUN | |
| ADB devices | NOT_RUN | |
| Snapshot logcat | NOT_RUN | |
| Smoke test | NOT_RUN | |

## Logi i diagnostyka

| Test | Status | Uwagi/defekt |
| --- | --- | --- |
| Ekran `Logi i diagnostyka` otwiera się | NOT_RUN | |
| `LOCAL_TEST_MARKER` pojawia się w podglądzie | NOT_RUN | |
| Plik JSONL ma poprawny format | NOT_RUN | |
| E-mail i token są zamaskowane | NOT_RUN | |
| Rotacja po przekroczeniu limitu pliku | NOT_RUN | |
| Retencja i limit liczby plików | NOT_RUN | |
| Eksport ZIP działa | NOT_RUN | |
| Manifest zawiera build i urządzenie | NOT_RUN | |
| Manifest zawiera health check SQLite | NOT_RUN | |
| Eksport nie wymaga sieci | NOT_RUN | |
| `pull-android-diagnostics.ps1` pobiera logi Debug | NOT_RUN | |
| Skrypt zapisuje logcat i dumpsys | NOT_RUN | |
| Crash lub wyjątek trafia do logu | NOT_RUN | |
| Czyszczenie logów działa | NOT_RUN | |

## Lokalna baza SQLite

| Test | Status | Uwagi/defekt |
| --- | --- | --- |
| Nowa instalacja tworzy bazę | NOT_RUN | |
| Historia pozostaje po restarcie | NOT_RUN | |
| Ulubione pozostają po restarcie | NOT_RUN | |
| Duplikat historii jest zastępowany | NOT_RUN | |
| Duplikat ulubionego nie jest dodawany | NOT_RUN | |
| Usuwanie pojedynczego ulubionego | NOT_RUN | |
| Czyszczenie historii | NOT_RUN | |
| Czyszczenie ulubionych | NOT_RUN | |
| Migracja `Preferences -> SQLite` | NOT_RUN | |
| Migracja nie duplikuje wpisów | NOT_RUN | |

## Macierz 20 × 5

| Projekt | SCN-01 | SCN-02 | SCN-03 | SCN-04 | SCN-05 | Wynik | Uwagi/defekt |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `plama-ratownik` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `kolek-dobieracz` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `pies-trener-7dni` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `bajka-z-rysunku` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `vinted-olx-opis` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `kot-bawi-sie` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `barber-translator` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `outfit-coach` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `domfix` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `fryzury-proste` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `rysunek-coach` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `bukietownik` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `pokoj-makeover` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `pakowanie-paczek` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `silikon-fuga-fix` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `szydelko-pomocnik` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `chleb-zakwas-coach` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `zmywarka-diagnosta` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `krawat-garnitur-coach` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |
| `router-wifi-diagnosta` | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | NOT_RUN | 0/5 | |

## Regresja końcowa

| Obszar | Status | Uwagi |
| --- | --- | --- |
| Start offline | NOT_RUN | |
| Systemowy Wstecz | NOT_RUN | |
| Obrót ekranu | NOT_RUN | |
| Tło i wznowienie | NOT_RUN | |
| Odmowa FilePicker/uprawnień | NOT_RUN | |
| Długi tekst PL/EN/UK | NOT_RUN | |
| Powiększony tekst systemowy | NOT_RUN | |
| Mała ilość miejsca | NOT_RUN | |
| ONNX na urządzeniu | NOT_RUN | |
| Eksport diagnostyki offline | NOT_RUN | |

## Podsumowanie defektów

| ID | Priorytet | Projekt | Scenariusz | Czas | Sesja logów | Dowód | Status | Opis |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| | | | | | | | | |

## Kryterium wydania

Wersja może otrzymać status `production-ready`, gdy:

- wszystkie testy automatyczne są zielone,
- Android Debug i Release build przechodzą,
- testy logowania, eksportu ZIP i pobierania przez ADB mają `PASS`,
- wszystkie 100 scenariuszy ma status `PASS`,
- wszystkie testy SQLite, w tym migracja, mają `PASS`,
- nie ma otwartych defektów krytycznych ani wysokich,
- modele Local AI są skonfigurowane i przetestowane albo funkcje AI są wyłączone w buildzie publikacyjnym,
- wykonano regresję po ostatniej poprawce.
