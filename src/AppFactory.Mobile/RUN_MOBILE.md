# Uruchomienie AppFactory.Mobile

## Cel

Ten projekt jest szkieletem wspólnego silnika Android dla repo `AplikacjeAndroid`.

Pierwsza podpięta aplikacja testowa:

```text
plama-ratownik
```

## Wymagania

- .NET 9 SDK
- workload MAUI
- Visual Studio 2022 z MAUI albo CLI z workloadami
- Android SDK / emulator albo telefon Android

## Sprawdzenie workloadów

```powershell
dotnet workload list
```

Jeżeli brakuje MAUI:

```powershell
dotnet workload install maui
```

## Build Android

Z katalogu repo:

```powershell
dotnet build .\src\AppFactory.Mobile\AppFactory.Mobile.csproj -f net9.0-android
```

## Co powinno działać w MVP

1. Start aplikacji.
2. Ekran główny pokazuje nazwę `Plama Ratownik`.
3. Przejście do kategorii.
4. Wybór kategorii.
5. Quiz z pytaniami.
6. Dopasowanie reguły.
7. Wynik podstawowy.
8. Mock reklamy rewarded.
9. Wynik pełny.
10. Dodanie do ulubionych.
11. Historia.

## Ważne

Na tym etapie reklamy są mockiem. Nie ma jeszcze AdMob, SQLite ani tłumaczeń runtime. To jest pierwszy szkielet funkcjonalny silnika.

## Następne kroki

- sprawdzić kompilację,
- poprawić błędy namespace/importów,
- dodać SQLite,
- dodać tłumaczenia,
- dodać prawdziwy AdMob jako osobny moduł,
- przygotować wariant builda pod Google Play.
