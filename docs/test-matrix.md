# Test Matrix

| Use Case | Юніт-тести | Інтеграційні тести |
|---|---|---|
| Оренда авто | RentCar_ValidData, RentCar_MakesCarUnavailable, RentCar_UnavailableCar | FullCycle_RentReturnReload |
| Повернення авто | ReturnCar_CompletesRental | SaveAndReload_Rental_PreservesStatus |
| Скасування оренди | CancelRental_Cancels | - |
| Розрахунок вартості | Rental_CalculateCost, Theory (1/3/7/30 днів) | - |
| Discount стратегія | RentCar_WithDiscountStrategy, DiscountPricing_10Percent | - |
| JSON збереження | - | SaveAndReload_Cars, SaveAndReload_Clients |
| Відновлення стану | - | SaveAndReload_CarAvailability |
| Пошкоджений JSON | - | LoadAsync_CorruptedJson |
| Відсутній файл | - | LoadAsync_MissingFile |
| Кілька збережень | - | MultipleSequentialSaves |
| Негативні сценарії | Car_InvalidData (5 Theory), Client_InvalidData (3 Theory), Rental_NullCar/Client | - |