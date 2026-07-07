# Shared Engine — wspólny silnik aplikacji

## Cel

`shared-engine` opisuje wspólny fundament dla wszystkich aplikacji w repozytorium.

Silnik ma pozwalać tworzyć kolejne aplikacje przez zmianę danych, a nie przez przepisywanie kodu.

## Główne funkcje

1. wybór języka,
2. lista kategorii,
3. quiz / formularz,
4. lokalne reguły,
5. wynik podstawowy,
6. odblokowanie pełnego wyniku,
7. historia,
8. ulubione,
9. eksport / udostępnienie,
10. reklamy / premium.

## Brak serwera

MVP nie korzysta z własnego backendu. Wszystkie dane projektowe są lokalne.

## Dane projektu

Każdy projekt ma mieć pliki:

- `projects/{projectId}/README.md`
- `projects/{projectId}/PROMPT_CODEX.md`
- `projects/{projectId}/data/app.json`
- `projects/{projectId}/data/categories.json`
- `projects/{projectId}/data/questions.json`
- `projects/{projectId}/data/rules.json`
- `projects/{projectId}/data/results.pl.json`
- `projects/{projectId}/data/i18n/pl.json`
- `projects/{projectId}/marketing/store-listing.pl.md`
- `projects/{projectId}/tests/manual-tests.md`

## Przyszłe rozszerzenia AI

Silnik ma być gotowy na przyszłe moduły:

- OCR,
- analiza zdjęcia,
- analiza dźwięku,
- lokalny model,
- zdalne API AI.

W MVP używany jest tryb bez AI. Wynik powstaje na podstawie pytań, reguł i lokalnych danych.

## Priorytet

Najpierw działający prosty produkt, potem AI.
