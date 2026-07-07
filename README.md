# AplikacjeAndroid

Repozytorium centralne dla serii prostych aplikacji Android budowanych bez serwera, lokalnie, w technologii .NET MAUI Blazor Hybrid.

## Cel

Zbudować fabrykę mikroaplikacji, w której każda aplikacja działa według schematu:

```text
użytkownik wybiera kategorię
→ odpowiada na pytania
→ aplikacja lokalnie dobiera wynik z danych JSON
→ pokazuje wynik podstawowy
→ pełny wynik odblokowuje reklama rewarded albo wersja premium
```

## Założenia

- Android first.
- Brak własnego serwera.
- Dane lokalne w JSON/SQLite.
- Brak trenowania AI na MVP.
- Gotowość pod przyszłe moduły AI: OCR, analiza zdjęcia, analiza dźwięku, lokalny model.
- Obsługa języków: PL, EN, UK na start; struktura pod wszystkie języki UE.
- Jeden wspólny silnik aplikacji i wiele projektów z osobnymi danymi.

## Struktura

```text
docs/                 dokumentacja nadrzędna i prompty dla Codexa
shared-engine/        opis wspólnego silnika aplikacji
projects/             osobne katalogi projektów
marketing/            materiały marketingowe wspólne i projektowe
tests/                scenariusze testowe wspólne
```

## Projekty priorytetowe

1. Plama Ratownik
2. Kołek Dobieracz
3. Pies Trener 7 Dni
4. Fryzury do szkoły
5. Bajka z rysunku dziecka
6. Vinted/OLX Opis
7. Kot Bawi się
8. Barber Translator
9. Outfit Coach
10. Pakowanie Paczek

## Technologia docelowa

```text
.NET 9 / .NET MAUI
Blazor Hybrid
SQLite
JSON data packs
AdMob rewarded ads
Google Play Billing opcjonalnie
Google UMP dla zgód reklamowych w UE
```
