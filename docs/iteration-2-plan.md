# Ітерація 2 — План (Lab 35)

## Сценарії з backlog що переходять у реалізацію
1. Збереження і відновлення стану з JSON-файлу
2. Оренда авто з перевіркою доступності (розширення Lab 34)
3. Повернення авто з розрахунком вартості
4. Скасування оренди клієнтом
5. Пошук і фільтрація авто та оренд

## Класи з Lab 34 що залишаються без змін
- Car, Client, Rental, RentalStatus (доменна модель)
- ICarRepository, IClientRepository, IRentalRepository (інтерфейси)
- InMemoryCarRepository, InMemoryClientRepository, InMemoryRentalRepository

## Точки розширення що планую використати
- ICarRepository → додамо JsonCarRepository
- IClientRepository → додамо JsonClientRepository
- IRentalRepository → додамо JsonRentalRepository
- RentalService → додамо нові методи без зміни існуючих
- Strategy патерн для розрахунку вартості оренди (тарифи)

## Ризики
- Серіалізація Guid і DateTime потребує налаштування
- Інваріанти Rental не можна порушити при десеріалізації
- Дублювання логіки між меню і сервісами треба уникати