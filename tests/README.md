# Testy — AplikacjeAndroid

## Cel

Testy mają pilnować dwóch rzeczy:

1. logiki wspólnego silnika,
2. integralności lokalnych danych projektowych.

## Uruchomienie

Z katalogu głównego repo:

```powershell
dotnet test .\tests\AppFactory.Mobile.Tests\AppFactory.Mobile.Tests.csproj
```

## Aktualne testy

### RuleEngineServiceTests

Sprawdza:

- dopasowanie szczegółowej reguły,
- fallback do reguły domyślnej.

### PlamaRatownikDataIntegrityTests

Sprawdza:

- czy istnieją wymagane pliki danych,
- czy kategorie nie są puste,
- czy pytania nie są puste,
- czy reguły nie są puste,
- czy wyniki nie są puste,
- czy reguły wskazują istniejące kategorie,
- czy reguły wskazują istniejące wyniki,
- czy warunki reguł wskazują istniejące pytania.

## Następne testy

- test pełnego scenariusza: coffee/cotton/yes -> wynik kawy,
- test pełnego scenariusza: blood/any/yes -> wynik krwi,
- test danych dla kolejnych projektów,
- test braku duplikatów ID,
- test braku pustych kroków w wynikach.
