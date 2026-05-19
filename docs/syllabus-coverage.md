# Syllabus Coverage Matrix

## Основи ООП
| Тема | Статус | Де використано |
|---|---|---|
| Класи і об'єкти | ✅ Повністю | Car, Client, Rental, RentalService |
| Конструктори | ✅ Повністю | Всі доменні класи, валідація в конструкторі |
| Інкапсуляція | ✅ Повністю | private set у всіх властивостях Domain |
| Спадкування | ✅ Частково | IDisposable в IntegrationTests |
| Перевизначення методів | ✅ Повністю | ToString() у Car, Client, Rental |

## Абстракції, поліморфізм, інтерфейси
| Тема | Статус | Де використано |
|---|---|---|
| Інтерфейси | ✅ Повністю | ICarRepository, IClientRepository, IRentalRepository, IPricingStrategy, ICommand |
| Поліморфізм | ✅ Повністю | Strategy патерн, Command патерн |
| Абстрактні класи | ➖ Не використано | Замінено інтерфейсами |

## Generics, колекції, LINQ, делегати
| Тема | Статус | Де використано |
|---|---|---|
| Generics | ✅ Повністю | List<T>, Dictionary<string, IPricingStrategy> |
| LINQ | ✅ Повністю | 5 запитів у RentalService |
| Dictionary | ✅ Повністю | Стратегії тарифів, команди меню |
| Делегати / Func | ✅ Повністю | Func<Task> saveAsync в командах |

## Обробка помилок і persistence
| Тема | Статус | Де використано |
|---|---|---|
| Винятки | ✅ Повністю | ArgumentException, InvalidOperationException в домені |
| try/catch | ✅ Повністю | AppRunner, JsonRepository.LoadAsync |
| Async/await | ✅ Повністю | SaveAsync, LoadAsync, ExecuteAsync |
| JSON серіалізація | ✅ Повністю | JsonCarRepository, JsonClientRepository, JsonRentalRepository |

## SOLID
| Принцип | Статус | Де демонструється |
|---|---|---|
| Single Responsibility | ✅ Повністю | Кожен клас має одну відповідальність |
| Open/Closed | ✅ Повністю | Нові тарифи і команди без зміни існуючого коду |
| Liskov Substitution | ✅ Повністю | JsonRepository замінює InMemoryRepository |
| Interface Segregation | ✅ Повністю | Окремі інтерфейси для Car, Client, Rental |
| Dependency Inversion | ✅ Повністю | RentalService залежить від інтерфейсів |

## Патерни
| Патерн | Статус | Де використано |
|---|---|---|
| Repository | ✅ Повністю | ICarRepository, IClientRepository, IRentalRepository |
| Strategy | ✅ Повністю | IPricingStrategy, StandardPricingStrategy, DiscountPricingStrategy |
| Command | ✅ Повністю | ICommand, AppRunner, 8 команд |

## UML
| Тема | Статус | Де використано |
|---|---|---|
| Діаграма класів | ✅ Повністю | docs/class-diagram.md |
| Діаграма послідовності | ✅ Повністю | docs/sequence-diagram.md |

## Тестування
| Тема | Статус | Де використано |
|---|---|---|
| xUnit | ✅ Повністю | 50 тестів |
| Theory / InlineData | ✅ Повністю | CarRentalTheoryTests |
| Інтеграційні тести | ✅ Повністю | IntegrationTests (8 тестів) |
| Coverage | ✅ Повністю | coverlet.msbuild |
| Test pyramid | ✅ Повністю | 42 юніт + 8 інтеграційних |

## Рефакторинг
| Тема | Статус | Де використано |
|---|---|---|
| Виділення методів | ✅ Повністю | TestHelpers.CreateService() |
| Виділення класів | ✅ Повністю | Command патерн з Program.cs |
| Усунення дублювання | ✅ Повністю | TestHelpers, AppRunner |
| Smell hunting | ✅ Повністю | Switch → Dictionary, 200 рядків → 46 |