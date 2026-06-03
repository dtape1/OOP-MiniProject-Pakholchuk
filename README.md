# Car Rental System

Система управління орендою автомобілів, розроблена як навчальний капстоун з ООП.
Система управління орендою автомобілів — оновлено в main.

## Технології
- .NET 9 / C#
- xUnit + coverlet (тестування)
- GitHub Actions (CI)
- System.Text.Json (persistence)

## Запуск
```powershell
git clone https://github.com/dtape1/OOP-MiniProject-Pakholchuk.git
cd OOP-MiniProject-Pakholchuk
dotnet run --project src/CarRental.Console
```

## Тести
```powershell
dotnet test
```

## Документація
- [User Guide](USER_GUIDE.md) — як користуватись застосунком
- [Developer Guide](DEVELOPER_GUIDE.md) — архітектура і правила розширення
- [Final Report](FINAL_REPORT.md) — технічний звіт
- [Changelog](CHANGELOG.md) — історія змін
- [Testing](TESTING.md) — стратегія і результати тестування
- [Demo](DEMO.md) — сценарій демонстрації

## Структура проєкту
- `src/CarRental.Domain` — сутності, інтерфейси
- `src/CarRental.Application` — бізнес-логіка, тарифи
- `src/CarRental.Infrastructure` — JSON і InMemory репозиторії
- `src/CarRental.Console` — консольне меню, команди
- `tests/CarRental.Tests` — юніт і інтеграційні тести
- `docs/` — діаграми, ітерації, тестова стратегія