# Google Play Data Safety — AppFactory Pomocniki 1.0

> Dokument roboczy do przepisania w Play Console. Właściciel konta odpowiada za zgodność odpowiedzi z finalnym AAB i wszystkimi użytymi SDK.

## Zakres binarki

- package ID: `pl.gbcom.appfactory`,
- wersja: `1.0.0`,
- reklamy: wyłączone,
- zewnętrzna analityka: brak,
- konto użytkownika: brak,
- Local AI obrazu i audio: wyłączone w domyślnym buildzie Release,
- historia, ulubione, ustawienia i logi: wyłącznie lokalne,
- eksport logów: ręczna akcja użytkownika przez systemowy arkusz udostępniania.

## Proponowane odpowiedzi

### Czy aplikacja zbiera lub udostępnia wymagane typy danych użytkownika?

`Nie` — dla wersji 1.0 opisanej powyżej.

Uzasadnienie: w terminologii Google Play „zbieranie” oznacza przesłanie danych poza urządzenie. Aplikacja nie wysyła danych do wydawcy ani usługi analitycznej. Ręczny eksport pliku do miejsca wybranego przez użytkownika nie jest automatycznym zbieraniem przez wydawcę.

### Czy wszystkie dane są szyfrowane podczas transmisji?

`Nie dotyczy`, ponieważ aplikacja nie przesyła danych użytkownika do własnego serwera.

### Czy użytkownik może zażądać usunięcia danych?

Dla danych przechowywanych wyłącznie lokalnie:

- historia ma akcję czyszczenia,
- ulubione mają akcję czyszczenia i usuwania pojedynczego wpisu,
- logi mają akcję czyszczenia,
- pełny reset jest dostępny przez ustawienia Androida lub odinstalowanie.

W formularzu należy odpowiedzieć zgodnie z aktualnym brzmieniem pytania. Aplikacja nie utrzymuje zdalnego konta ani zdalnego magazynu danych.

### Niezależna weryfikacja bezpieczeństwa

`Nie` — dopóki aplikacja nie przejdzie formalnego audytu MASA lub równoważnego.

## Dane, których nie należy deklarować jako zbierane

Przy obecnej konfiguracji Release:

- zdjęcia,
- audio,
- e-mail użytkownika,
- identyfikatory urządzenia,
- lokalizacja,
- kontakty,
- aktywność w aplikacji,
- informacje o awariach,
- diagnostyka,
- pliki użytkownika.

Są one albo nieużywane, albo pozostają lokalnie i nie są automatycznie transmitowane.

## Ręczny eksport diagnostyki

Paczka ZIP może zawierać:

- wersję aplikacji,
- model i wersję systemu urządzenia,
- identyfikator lokalnej sesji,
- health check SQLite,
- lokalne logi.

Eksport:

- jest dobrowolny,
- wymaga jawnego kliknięcia,
- używa systemowego arkusza udostępniania,
- nie ma domyślnego odbiorcy,
- nie jest wysyłany automatycznie do GBCom.

## Warunek ponownej oceny

Formularz i politykę trzeba ponownie przeanalizować przed włączeniem któregokolwiek z elementów:

- AdMob lub innej sieci reklamowej,
- Firebase Analytics/Crashlytics,
- zdalnego API,
- pobierania modeli połączonego z własną telemetryką,
- kont użytkowników,
- synchronizacji danych,
- Local AI wysyłającego pliki poza urządzenie,
- zewnętrznego systemu obsługi błędów.

## Kontrola finalnego AAB

Przed wysłaniem formularza:

1. sprawdź zależności NuGet,
2. sprawdź wygenerowany manifest i uprawnienia,
3. sprawdź ruch sieciowy wersji Release,
4. potwierdź, że `EnableLocalAiRelease=false`,
5. potwierdź, że `AdsEnabled=false`,
6. potwierdź brak zewnętrznych endpointów telemetrycznych,
7. porównaj wynik z publiczną polityką prywatności.
