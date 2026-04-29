# Car Rental System
Система управління орендою автомобілів, розроблена в рамках курсу ООП.

## Технології
- .NET 9 / C#
- xUnit (тестування)
- GitHub Actions (CI)

## Структура проєкту

- `src/CarRental.Domain` — сутності, інтерфейси
- `src/CarRental.Application` — бізнес-логіка
- `src/CarRental.Infrastructure` — in-memory репозиторії
- `src/CarRental.Console` — консольний інтерфейс
- `tests/CarRental.Tests` — юніт-тести
- `docs/` — документація

## Запуск
```powershell
dotnet run --project src/CarRental.Console
```

## Тести
```powershell
dotnet test
```

## Функціональність
- Перегляд доступних автомобілів
- Перегляд клієнтів
- Оформлення оренди з розрахунком вартості
- Повернення автомобіля
- Перегляд активних оренд