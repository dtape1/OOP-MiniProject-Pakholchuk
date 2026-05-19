# Developer Guide — Car Rental System

## Вимоги
- .NET 9 SDK
- Git

## Швидкий старт
```powershell
git clone https://github.com/dtape1/OOP-MiniProject-Pakholchuk.git
cd OOP-MiniProject-Pakholchuk
dotnet build
dotnet run --project src/CarRental.Console
```

## Структура проєкту
- `src/CarRental.Domain` — сутності і інтерфейси, нульова залежність
- `src/CarRental.Application` — бізнес-логіка, залежить тільки від Domain
- `src/CarRental.Infrastructure` — JSON і InMemory репозиторії
- `src/CarRental.Console` — точка входу, команди меню
- `tests/CarRental.Tests` — юніт і інтеграційні тести

## Архітектура

### Шари і залежності
Console → Application → Domain
Infrastructure → Domain
Infrastructure → Application
Tests → Domain, Application, Infrastructure

### Ключові класи
- `RentalService` — вся бізнес-логіка оренди
- `AppRunner` — цикл меню, делегує виконання командам
- `ICommand` — інтерфейс команди меню
- `IPricingStrategy` — інтерфейс стратегії розрахунку ціни

## Як додати новий тариф
1. Створи клас в `src/CarRental.Application/Pricing/`
2. Реалізуй `IPricingStrategy`
3. Зареєструй в `Program.cs`:
```csharp
service.RegisterPricingStrategy(new MyNewStrategy());
```
Більше нічого міняти не треба.

## Як додати нову команду меню
1. Створи клас в `src/CarRental.Console/Commands/`
2. Реалізуй `ICommand` (Key, Description, ExecuteAsync)
3. Додай в масив команд у `Program.cs`

## Як додати новий репозиторій
1. Реалізуй відповідний інтерфейс з `CarRental.Domain.Interfaces`
2. Підстав в `Program.cs` замість поточного репозиторію
Бізнес-логіка не зміниться.

## Запуск тестів
```powershell
# Базовий запуск
dotnet test

# З coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Правила розширення
- Нова логіка → новий метод в `RentalService`, не в командах
- Нове сховище → нова реалізація інтерфейсу, не зміна існуючого
- Нова поведінка розрахунку → новий Strategy клас
- Команди меню не містять бізнес-логіки — тільки UI