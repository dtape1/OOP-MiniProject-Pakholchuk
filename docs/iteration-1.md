# Ітерація 1 — Lab 34

## Що вже працює
- Доменна модель: Car, Client, Rental, RentalStatus
- Інтерфейси репозиторіїв: ICarRepository, IClientRepository, IRentalRepository
- In-memory репозиторії
- RentalService з методами RentCar, ReturnCar, GetActiveRentals
- Консольне меню з повним вертикальним зрізом
- 10 юніт-тестів (усі проходять)
- GitHub Actions CI

## Артефакти в репозиторії
- docs/vision.md
- docs/backlog.md
- docs/class-diagram.md
- docs/sequence-diagram.md
- docs/iteration-1.md
- src/ — повна структура solution
- tests/ — юніт-тести
- .github/workflows/dotnet.yml

## Сценарії для розширення в Lab 35
1. Збереження даних у JSON-файл (persistence)
2. LINQ-запити — фільтрація авто за ціною, пошук оренд клієнта
3. Скасування оренди через меню

## Ризики та невизначеності
- Серіалізація Guid і DateTime потребує перевірки
- Структура JSON-файлів буде визначена в Lab 35

## Класи підготовлені під розширення
- ICarRepository — легко замінити InMemory на FileRepository
- RentalService — готовий до додавання нових сценаріїв
- Rental.CalculateCost() — готовий до Strategy патерну для різних тарифів