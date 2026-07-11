# Lokalna baza danych AppFactory

## Decyzja

Do aplikacji dodano lokalną bazę SQLite.

Baza jest potrzebna dla danych, które:

- rosną podczas używania aplikacji,
- wymagają sortowania i filtrowania,
- muszą pozwalać na usuwanie pojedynczych rekordów,
- powinny przeżyć restart procesu aplikacji,
- wymagają kontrolowanej migracji schematu.

SQLite przechowuje:

- historię wyników,
- ulubione wyniki,
- wersję schematu bazy.

`Preferences` pozostają dla małych danych klucz-wartość:

- wybranego języka i ustawień,
- flag wykonanych migracji,
- licznika rzędów,
- krótkich notatek projektu szydełkowego.

## Projekty

Warstwa trwałości znajduje się w:

```text
src/AppFactory.Persistence/AppFactory.Persistence.csproj
```

Aplikacja mobilna odwołuje się do niej przez `ProjectReference`.

Pakiet:

```text
sqlite-net-pcl 1.9.172
```

Plik bazy na urządzeniu:

```text
<FileSystem.AppDataDirectory>/appfactory.db3
```

## Schemat v1

### `history`

- `Id` — klucz główny,
- `ProjectId`,
- `CategoryId`,
- `ResultId`,
- `FreeResultId`,
- `PremiumResultId`,
- `CreatedAtUtcTicks`.

Historia:

- jest sortowana malejąco po czasie,
- deduplikuje ten sam projekt, kategorię i wynik,
- przechowuje maksymalnie 100 najnowszych wpisów.

### `favorites`

- `Id` — klucz główny,
- `ProjectId`,
- `CategoryId`,
- `ResultId`,
- `FreeResultId`,
- `PremiumResultId`,
- `CreatedAtUtcTicks`.

Ulubione:

- nie pozwalają dodać drugi raz tego samego wyniku projektu,
- można usuwać pojedynczo,
- można wyczyścić w całości.

### `schema_info`

- `Id`,
- `Version`,
- `UpdatedAtUtcTicks`.

Aktualna wersja:

```text
1
```

## Migracja danych

`HistoryService` i `FavoritesService` wykonują jednorazową migrację wcześniejszych list JSON z `Preferences`.

Proces:

1. inicjalizacja SQLite,
2. odczyt starszego JSON,
3. zapis rekordów do tabel,
4. usunięcie starego klucza,
5. zapis flagi zakończenia migracji.

Uszkodzony starszy JSON nie blokuje uruchomienia aplikacji.

## Testy automatyczne

Plik:

```text
tests/AppFactory.Mobile.Tests/AppDatabaseTests.cs
```

Zakres:

- utworzenie schematu,
- health check bazy,
- sortowanie historii,
- deduplikacja historii,
- limit 100 wpisów,
- deduplikacja ulubionych,
- usuwanie pojedynczego ulubionego,
- czyszczenie ulubionych.

Uruchomienie:

```powershell
dotnet test tests/AppFactory.Mobile.Tests/AppFactory.Mobile.Tests.csproj -c Release --filter AppDatabaseTests
```

## Test manualny migracji

1. Uruchom wcześniejszą wersję aplikacji zapisującą historię i ulubione w `Preferences`.
2. Dodaj minimum dwa wpisy historii i jeden ulubiony.
3. Zainstaluj nową wersję bez czyszczenia danych aplikacji.
4. Otwórz historię i ulubione.
5. Sprawdź, czy wpisy nadal istnieją i można je ponownie otworzyć.
6. Uruchom aplikację ponownie i sprawdź dane drugi raz.

## Reset przed czystą sesją testową

Najprostszy reset:

- wyczyść dane aplikacji w ustawieniach Androida,
- albo odinstaluj aplikację i zainstaluj ją ponownie.

Nie usuwaj samego pliku `appfactory.db3` podczas działania procesu aplikacji.
