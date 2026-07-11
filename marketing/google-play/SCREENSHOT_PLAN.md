# Screenshot plan — AppFactory Pomocniki

## Wymagany zestaw telefonu

Docelowy format:

- orientacja: pionowa,
- rekomendowany rozmiar: `1080×1920`,
- format: PNG bez przezroczystości,
- źródło: finalny build Release lub release candidate,
- liczba: 6,
- brak ramek urządzenia, ocen, ceny i haseł typu „najlepsza”.

Katalog surowy:

```text
artifacts/google-play/screenshots/raw
```

Katalog zatwierdzony:

```text
artifacts/google-play/screenshots/final
```

## Kolejność

### 01 — Katalog pomocników

Ekran:

- strona główna,
- widoczny nagłówek AppFactory,
- widoczne karty kilku różnych kategorii,
- pole wyszukiwania bez wpisanego prywatnego tekstu.

Nazwa:

```text
01-catalog.png
```

Alt PL:

`Katalog praktycznych pomocników do domu, stylu, hobby i codziennych problemów.`

Alt EN:

`Catalog of practical helpers for home, style, hobbies and everyday problems.`

### 02 — Wybór kategorii

Ekran:

- projekt `plama-ratownik` albo `zmywarka-diagnosta`,
- hero projektu,
- lista kategorii,
- brak panelu Local AI w buildzie Release 1.0.

Nazwa:

```text
02-categories.png
```

Alt PL:

`Ekran pomocnika z wyborem rodzaju problemu przed rozpoczęciem quizu.`

### 03 — Quiz

Ekran:

- pytanie z co najmniej trzema odpowiedziami,
- widoczny pasek postępu,
- widoczne przyciski cofnięcia i resetu.

Nazwa:

```text
03-quiz.png
```

Alt PL:

`Krótki quiz z paskiem postępu i dużymi przyciskami odpowiedzi.`

### 04 — Dopasowany wynik

Ekran:

- pełna rekomendacja,
- kilka kroków,
- widoczne „Dlaczego taki wynik?”,
- brak przycisku reklamy lub płatności.

Nazwa:

```text
04-result.png
```

Alt PL:

`Dopasowana rekomendacja krok po kroku z wyjaśnieniem wyboru.`

### 05 — Historia i ulubione

Ekran:

- lista historii albo ulubionych,
- minimum dwa bezpieczne wpisy testowe,
- widoczna akcja ponownego otwarcia.

Nazwa:

```text
05-history-favorites.png
```

Alt PL:

`Lokalna historia wyników umożliwiająca ponowne otwarcie rekomendacji.`

### 06 — Prywatność i ustawienia

Ekran:

- ustawienia,
- stan SQLite,
- sekcja diagnostyki,
- linki do polityki prywatności i pomocy.

Nazwa:

```text
06-privacy-settings.png
```

Alt PL:

`Ustawienia z lokalną bazą danych, diagnostyką i kontrolą prywatności.`

## Procedura

1. Wyczyść dane testowe, które nie powinny być widoczne.
2. Ustaw język odpowiedni dla listingu.
3. Ustaw rozmiar tekstu systemowego na domyślny.
4. Wyłącz powiadomienia i elementy nakładane przez system.
5. Ustaw godzinę i poziom baterii tak, aby nie odciągały uwagi.
6. Przejdź do wymaganego ekranu.
7. Uruchom:

```powershell
pwsh ./tools/release/capture-play-screenshot.ps1 -Name 01-catalog
```

8. Powtórz dla sześciu ekranów.
9. Otwórz każdy PNG i sprawdź, czy nie ma danych osobowych, błędów UI ani treści debugowych.
10. Nie używaj screenshotów wykonanych przed ostatnią zmianą UI.

## Gate zatwierdzenia

Każdy plik musi:

- mieć poprawną orientację,
- pokazywać prawdziwy interfejs,
- nie zawierać Local AI, jeśli jest wyłączone w release,
- nie zawierać mockowanych reklam,
- nie zawierać wyjątków, identyfikatorów sesji i ścieżek plików,
- odpowiadać opisowi i aktualnej binarce.
