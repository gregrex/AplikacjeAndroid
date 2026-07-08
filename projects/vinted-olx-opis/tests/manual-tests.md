# Testy manualne — Opis Sprzedażowy

## 1. Przyjazny opis ubrania

Wybierz:

- kategoria: clothes,
- stan: very_good,
- ton: friendly,
- dostawa: both.

Oczekiwane:

- wynik `friendly_any`,
- opis brzmi naturalnie,
- tekst zawiera miejsce na stan, cechy i dostawę.

## 2. Krótki opis

Wybierz:

- dowolna kategoria,
- ton: short.

Oczekiwane:

- wynik `short_any`,
- opis jest zwięzły,
- użytkownik może go szybko skopiować i edytować.

## 3. Elektronika

Wybierz:

- kategoria: electronics,
- stan: good,
- ton: detailed.

Oczekiwane:

- wynik `electronics_good`,
- opis zawiera stan techniczny,
- jest ostrzeżenie, aby nie gwarantować sprawności bez testu.

## 4. Przedmiot używany

Wybierz:

- stan: used.

Oczekiwane:

- wynik `used_any`,
- opis zawiera informację o śladach użytkowania.

## 5. Historia i ulubione

Po pokazaniu wyniku:

- dodaj wynik do ulubionych,
- sprawdź historię.

Oczekiwane:

- wynik zapisuje się lokalnie,
- historia zawiera projekt i wynik.
