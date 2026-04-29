using CarRental.Application;
using CarRental.Application.Pricing;
using CarRental.Domain;
using CarRental.Infrastructure;

// JSON репозиторії
var carRepo = new JsonCarRepository("data/cars.json");
var clientRepo = new JsonClientRepository("data/clients.json");
var rentalRepo = new JsonRentalRepository("data/rentals.json", carRepo, clientRepo);

// Завантаження стану
await carRepo.LoadAsync();
await clientRepo.LoadAsync();
await rentalRepo.LoadAsync();

var service = new RentalService(carRepo, clientRepo, rentalRepo);

// Реєстрація стратегій
service.RegisterPricingStrategy(new StandardPricingStrategy());
service.RegisterPricingStrategy(new DiscountPricingStrategy(10));
service.RegisterPricingStrategy(new DiscountPricingStrategy(20));

// Тестові дані якщо база порожня
if (!service.GetAllClients().Any())
{
    service.AddCar(new Car("Toyota", "Camry", 2021, 800));
    service.AddCar(new Car("BMW", "X5", 2022, 1500));
    service.AddCar(new Car("Renault", "Logan", 2020, 500));
    service.AddClient(new Client("Олег Мельник", "oleg@email.com", "+380501234567"));
    service.AddClient(new Client("Аня Бондар", "anya@email.com", "+380671234567"));
    await carRepo.SaveAsync();
    await clientRepo.SaveAsync();
}

while (true)
{
    Console.WriteLine("\n=== Система оренди автомобілів ===");
    Console.WriteLine("1. Переглянути доступні авто");
    Console.WriteLine("2. Переглянути клієнтів");
    Console.WriteLine("3. Орендувати авто");
    Console.WriteLine("4. Повернути авто");
    Console.WriteLine("5. Скасувати оренду");
    Console.WriteLine("6. Активні оренди");
    Console.WriteLine("7. Аналітика");
    Console.WriteLine("8. Зберегти дані");
    Console.WriteLine("0. Вихід");
    Console.Write("Оберіть опцію: ");

    var input = Console.ReadLine();

    try
    {
        switch (input)
        {
            case "1":
                var cars = service.GetCarsSortedByPrice();
                if (!cars.Any()) { Console.WriteLine("Немає авто."); break; }
                Console.WriteLine("\nАвтомобілі (за ціною):");
                for (int i = 0; i < cars.Count; i++)
                    Console.WriteLine($"  {i + 1}. [{cars[i].Id.ToString()[..8]}] {cars[i]} {(cars[i].IsAvailable ? "✓" : "✗ зайнятий")}");
                break;

            case "2":
                var clients = service.GetAllClients();
                if (!clients.Any()) { Console.WriteLine("Немає клієнтів."); break; }
                Console.WriteLine("\nКлієнти:");
                for (int i = 0; i < clients.Count; i++)
                    Console.WriteLine($"  {i + 1}. [{clients[i].Id.ToString()[..8]}] {clients[i]}");
                break;

            case "3":
                var availCars = service.GetAvailableCars();
                var allClients = service.GetAllClients();
                if (!availCars.Any()) { Console.WriteLine("Немає доступних авто."); break; }
                if (!allClients.Any()) { Console.WriteLine("Немає клієнтів."); break; }

                Console.WriteLine("\nДоступні авто:");
                for (int i = 0; i < availCars.Count; i++)
                    Console.WriteLine($"  {i + 1}. {availCars[i]}");
                Console.Write("Оберіть номер авто: ");
                if (!int.TryParse(Console.ReadLine(), out int carIdx) || carIdx < 1 || carIdx > availCars.Count)
                { Console.WriteLine("Невірний вибір."); break; }

                Console.WriteLine("\nКлієнти:");
                for (int i = 0; i < allClients.Count; i++)
                    Console.WriteLine($"  {i + 1}. {allClients[i]}");
                Console.Write("Оберіть номер клієнта: ");
                if (!int.TryParse(Console.ReadLine(), out int clientIdx) || clientIdx < 1 || clientIdx > allClients.Count)
                { Console.WriteLine("Невірний вибір."); break; }

                Console.Write("Дата початку (dd.MM.yyyy): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime start))
                { Console.WriteLine("Невірна дата."); break; }

                Console.Write("Дата кінця (dd.MM.yyyy): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime end))
                { Console.WriteLine("Невірна дата."); break; }

                var strategies = service.GetAvailableStrategies();
                Console.WriteLine("\nТарифи:");
                for (int i = 0; i < strategies.Count; i++)
                    Console.WriteLine($"  {i + 1}. {strategies[i]}");
                Console.Write("Оберіть тариф (Enter = стандартний): ");
                var stratInput = Console.ReadLine();
                var stratName = "Стандартний";
                if (int.TryParse(stratInput, out int stratIdx) && stratIdx >= 1 && stratIdx <= strategies.Count)
                    stratName = strategies[stratIdx - 1];

                var rental = service.RentCar(allClients[clientIdx - 1].Id, availCars[carIdx - 1].Id, start, end, stratName);
                await carRepo.SaveAsync();
                await rentalRepo.SaveAsync();
                Console.WriteLine($"\n✓ Оренду оформлено!\n  {rental}");
                break;

            case "4":
                var activeRentals = service.GetActiveRentals();
                if (!activeRentals.Any()) { Console.WriteLine("Немає активних оренд."); break; }
                Console.WriteLine("\nАктивні оренди:");
                for (int i = 0; i < activeRentals.Count; i++)
                    Console.WriteLine($"  {i + 1}. {activeRentals[i]}");
                Console.Write("Оберіть номер для повернення: ");
                if (!int.TryParse(Console.ReadLine(), out int retIdx) || retIdx < 1 || retIdx > activeRentals.Count)
                { Console.WriteLine("Невірний вибір."); break; }
                var completed = service.ReturnCar(activeRentals[retIdx - 1].Id);
                await carRepo.SaveAsync();
                await rentalRepo.SaveAsync();
                Console.WriteLine($"\n✓ Авто повернено! Вартість: {completed.TotalCost} грн");
                break;

            case "5":
                var active2 = service.GetActiveRentals();
                if (!active2.Any()) { Console.WriteLine("Немає активних оренд."); break; }
                Console.WriteLine("\nАктивні оренди:");
                for (int i = 0; i < active2.Count; i++)
                    Console.WriteLine($"  {i + 1}. {active2[i]}");
                Console.Write("Оберіть номер для скасування: ");
                if (!int.TryParse(Console.ReadLine(), out int canIdx) || canIdx < 1 || canIdx > active2.Count)
                { Console.WriteLine("Невірний вибір."); break; }
                var cancelled = service.CancelRental(active2[canIdx - 1].Id);
                await carRepo.SaveAsync();
                await rentalRepo.SaveAsync();
                Console.WriteLine($"\n✓ Оренду скасовано: {cancelled}");
                break;

            case "6":
                var active3 = service.GetActiveRentals();
                if (!active3.Any()) { Console.WriteLine("Немає активних оренд."); break; }
                Console.WriteLine("\nАктивні оренди:");
                foreach (var r in active3)
                    Console.WriteLine($"  {r}");
                break;

            case "7":
                Console.WriteLine("\n=== Аналітика ===");
                Console.WriteLine($"Загальний дохід: {service.GetTotalRevenue()} грн");
                Console.WriteLine($"Доступних авто: {service.GetAvailableCars().Count}");
                Console.WriteLine($"Активних оренд: {service.GetActiveRentals().Count}");
                Console.Write("\nФільтр авто за ціною — мін (грн): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal min))
                {
                    Console.Write("Макс (грн): ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal max))
                    {
                        var filtered = service.GetCarsInPriceRange(min, max);
                        Console.WriteLine($"\nАвто від {min} до {max} грн/день:");
                        foreach (var c in filtered)
                            Console.WriteLine($"  {c}");
                    }
                }
                break;

            case "8":
                await carRepo.SaveAsync();
                await clientRepo.SaveAsync();
                await rentalRepo.SaveAsync();
                Console.WriteLine("✓ Дані збережено!");
                break;

            case "0":
                await carRepo.SaveAsync();
                await clientRepo.SaveAsync();
                await rentalRepo.SaveAsync();
                Console.WriteLine("До побачення!");
                return;

            default:
                Console.WriteLine("Невідома опція.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n⚠ Помилка: {ex.Message}");
    }
}