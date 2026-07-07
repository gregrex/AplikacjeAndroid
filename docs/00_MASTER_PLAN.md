# Master Plan — fabryka aplikacji Android

## 1. Strategia

Budujemy nie jedną aplikację, ale powtarzalny system do szybkiego tworzenia mikroaplikacji Android.

Każdy projekt ma mieć osobny katalog w `projects/`, ale korzystać z tych samych zasad:

- lokalne dane JSON,
- quiz/formularz,
- lokalny silnik reguł,
- wynik darmowy,
- odblokowanie pełnego wyniku reklamą rewarded,
- historia i ulubione lokalnie,
- obsługa wielu języków,
- brak własnego backendu.

## 2. Priorytet biznesowy

Najpierw projekty o niskim ryzyku i szybkim wdrożeniu:

1. `plama-ratownik`
2. `kolek-dobieracz`
3. `pies-trener-7dni`
4. `fryzury-do-szkoly`
5. `bajka-z-rysunku`

Druga fala:

6. `vinted-olx-opis`
7. `kot-bawi-sie`
8. `barber-translator`
9. `outfit-coach`
10. `pakowanie-paczek`

Trzecia fala:

11. `pokoj-makeover`
12. `domfix`
13. `zmywarka-diagnosta`
14. `router-wifi-diagnosta`
15. `szydelko-pomocnik`
16. `rysunek-coach`
17. `bukietownik`
18. `chleb-zakwas-coach`
19. `silikon-fuga-fix`
20. `krawat-garnitur-coach`

## 3. Plan 3 miesięcy

### Miesiąc 1 — fundament

- utworzyć projekt .NET MAUI Blazor Hybrid,
- zbudować nawigację,
- zbudować loader konfiguracji JSON,
- zbudować silnik quizów,
- zbudować silnik reguł,
- zbudować ekran wyniku,
- dodać SQLite,
- dodać historię i ulubione,
- przygotować `plama-ratownik` jako pierwszą aplikację testową.

### Miesiąc 2 — monetyzacja i paczki projektowe

- dodać AdMob rewarded ads,
- dodać ekran zgód reklamowych,
- dodać języki PL/EN/UK,
- przygotować projekty: `kolek-dobieracz`, `pies-trener-7dni`, `fryzury-do-szkoly`,
- przygotować scenariusze testowe.

### Miesiąc 3 — wdrożenie i powtarzalność

- przygotować build Android AAB,
- przygotować checklistę Google Play,
- przygotować materiały marketingowe,
- przygotować projekt `bajka-z-rysunku`,
- opisać proces klonowania aplikacji,
- przygotować backlog drugiej fali.

## 4. Zasada MVP

Nie trenujemy AI na starcie. Używamy:

- reguł,
- szablonów,
- gotowych scenariuszy,
- tekstów eksperckich,
- lokalnych danych.

AI dopiero po potwierdzeniu rynku.

## 5. Definicja ukończenia pierwszej wersji

Aplikacja bazowa jest gotowa, jeśli:

- uruchamia się na Androidzie,
- ładuje dane projektu z JSON,
- obsługuje quiz,
- dopasowuje regułę,
- pokazuje wynik darmowy i pełny,
- zapisuje historię,
- obsługuje ulubione,
- ma strukturę tłumaczeń,
- ma przygotowany moduł reklam,
- ma co najmniej jeden kompletny projekt danych.
