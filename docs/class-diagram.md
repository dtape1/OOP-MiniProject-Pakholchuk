# Діаграма класів — Car Rental System

```mermaid
classDiagram
    class Car {
        +Guid Id
        +string Brand
        +string Model
        +int Year
        +decimal PricePerDay
        +bool IsAvailable
        +Car(brand, model, year, pricePerDay, id?)
        +MakeAvailable()
        +MakeUnavailable()
    }

    class Client {
        +Guid Id
        +string FullName
        +string Email
        +string Phone
        +Client(fullName, email, phone, id?)
    }

    class Rental {
        +Guid Id
        +Car Car
        +Client Client
        +DateTime StartDate
        +DateTime EndDate
        +RentalStatus Status
        +decimal TotalCost
        +string PricingStrategyName
        +Rental(car, client, startDate, endDate, strategyName?, id?)
        +Complete()
        +Cancel()
        +CalculateCost() decimal
        +SetTotalCost(cost)
    }

    class RentalStatus {
        <<enumeration>>
        Active
        Completed
        Cancelled
    }

    class ICarRepository {
        <<interface>>
        +Add(car)
        +GetById(id) Car
        +GetAll() List~Car~
        +GetAvailable() List~Car~
    }

    class IClientRepository {
        <<interface>>
        +Add(client)
        +GetById(id) Client
        +GetAll() List~Client~
    }

    class IRentalRepository {
        <<interface>>
        +Add(rental)
        +GetById(id) Rental
        +GetAll() List~Rental~
        +GetActive() List~Rental~
    }

    class IPricingStrategy {
        <<interface>>
        +string Name
        +Calculate(pricePerDay, days) decimal
    }

    class StandardPricingStrategy {
        +string Name
        +Calculate(pricePerDay, days) decimal
    }

    class DiscountPricingStrategy {
        -decimal _discountPercent
        +string Name
        +Calculate(pricePerDay, days) decimal
    }

    class RentalService {
        -ICarRepository _cars
        -IClientRepository _clients
        -IRentalRepository _rentals
        -Dictionary _pricingStrategies
        +RegisterPricingStrategy(strategy)
        +RentCar(clientId, carId, start, end, strategy) Rental
        +ReturnCar(rentalId) Rental
        +CancelRental(rentalId) Rental
        +GetAvailableCars() List~Car~
        +GetCarsSortedByPrice() List~Car~
        +GetCarsInPriceRange(min, max) List~Car~
        +GetActiveRentals() List~Rental~
        +GetTotalRevenue() decimal
    }

    class ICommand {
        <<interface>>
        +string Key
        +string Description
        +ExecuteAsync() Task
    }

    class AppRunner {
        -Dictionary _commands
        +RunAsync() Task
        -PrintMenu()
    }

    Rental --> Car
    Rental --> Client
    Rental --> RentalStatus
    RentalService --> ICarRepository
    RentalService --> IClientRepository
    RentalService --> IRentalRepository
    RentalService --> IPricingStrategy
    StandardPricingStrategy ..|> IPricingStrategy
    DiscountPricingStrategy ..|> IPricingStrategy
    AppRunner --> ICommand
    ListCarsCommand ..|> ICommand
    ListClientsCommand ..|> ICommand
    RentCarCommand ..|> ICommand
    ReturnCarCommand ..|> ICommand
    CancelRentalCommand ..|> ICommand
    ActiveRentalsCommand ..|> ICommand
    AnalyticsCommand ..|> ICommand
    SaveDataCommand ..|> ICommand
```