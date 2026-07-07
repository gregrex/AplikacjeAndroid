# Codzienny workflow dla Codexa

## Cel

Codex ma pracować codziennie małymi paczkami, bez chaosu i bez przebudowywania wszystkiego naraz.

## Zasada pracy

Każda sesja Codexa ma mieć jeden konkretny cel:

- silnik,
- jeden projekt,
- jedna paczka danych,
- jeden moduł UI,
- jeden zestaw testów,
- jedna poprawka builda.

## Standardowy przebieg sesji

1. Przeczytaj `README.md`.
2. Przeczytaj `docs/00_MASTER_PLAN.md`.
3. Przeczytaj `docs/01_CODEX_PROMPT_SHARED_ENGINE.md`.
4. Jeżeli pracujesz nad projektem, przeczytaj jego `README.md` i `PROMPT_CODEX.md`.
5. Wykonaj tylko zakres wskazany w aktualnym zadaniu.
6. Nie usuwaj danych innych projektów.
7. Nie twórz backendu.
8. Nie trenuj AI.
9. Uruchom testy lub opisz, dlaczego nie da się ich uruchomić.
10. Zapisz raport pracy w `docs/progress/`.

## Priorytety codzienne

### Dzień 1–5

- utworzyć szkielet MAUI Blazor Hybrid,
- przygotować loader JSON,
- przygotować routing,
- przygotować modele danych,
- uruchomić projekt lokalnie.

### Dzień 6–10

- zaimplementować quiz,
- zaimplementować reguły,
- zaimplementować wynik,
- podłączyć `plama-ratownik`.

### Dzień 11–15

- historia,
- ulubione,
- ustawienia,
- wybór języka,
- mock reklam.

### Dzień 16–20

- dokończenie pierwszej aplikacji,
- testy manualne,
- materiały Google Play,
- przygotowanie builda Android.

## Raport Codexa po każdej sesji

Codex ma dopisać plik:

`docs/progress/YYYY-MM-DD_HHMM.md`

Format:

```text
# Raport pracy

## Cel sesji

## Zmienione pliki

## Co działa

## Co nie działa

## Testy

## Następny krok
```
