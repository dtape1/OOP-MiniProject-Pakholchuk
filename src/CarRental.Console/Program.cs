using CarRental.Application;
using CarRental.Domain;
using CarRental.Infrastructure;

var carRepo = new InMemoryCarRepository();
var clientRepo = new InMemoryClientRepository();
var rentalRepo = new InMemoryRentalRepository();
var service = new RentalService(carRepo, clientRepo, rentalRepo);

// Тестові дані
carRepo.Add(new Car("Toyota", "Camry", 2021, 800));
carRepo.Add(new Car("BMW", "X5", 2022, 1500));
carRepo.Add(new Car("Renault", "Logan", 2020, 500));
clientRepo.Add(new Client("Іван Міщук", "ivan@gmail.com", "+380501234567"));
clientRepo.Add(new Client("Марія Коваль", "maria@gmail.com", "+380671234567"));

while (true)
{
    Console.WriteLine("\n=== Система оренди автомобілів ===");
    Console.WriteLine("1. Переглянути доступні авто");
    Console.WriteLine("2. Переглянути клієнтів");
    Console.WriteLine("3. Орендувати авто");
    Console.WriteLine("4. Повернути авто");
    Console.WriteLine("5. Активні оренди");
    Console.WriteLine("0. Вихід");
    Console.Write("Оберіть опцію: ");

    var input = Console.ReadLine();

    try
    {
        switch (input)
        {
            case "1":
                var cars = service.GetAvailableCars();
                if (!cars.Any()) { Console.WriteLine("Немає доступних авто."); break; }
                Console.WriteLine("\nДоступні автомобілі:");
                for (int i = 0; i < cars.Count; i++)
                    Console.WriteLine($"  {i + 1}. [{cars[i].Id.ToString()[..8]}] {cars[i]}");
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

                var rental = service.RentCar(
                    allClients[clientIdx - 1].Id,
                    availCars[carIdx - 1].Id,
                    start, end);

                Console.WriteLine($"\n✓ Оренду оформлено!\n  {rental}");
                break;

            case "4":
                var activeRentals = service.GetActiveRentals();
                if (!activeRentals.Any()) { Console.WriteLine("Немає активних оренд."); break; }

                Console.WriteLine("\nАктивні оренди:");
                for (int i = 0; i < activeRentals.Count; i++)
                    Console.WriteLine($"  {i + 1}. {activeRentals[i]}");

                Console.Write("Оберіть номер оренди для повернення: ");
                if (!int.TryParse(Console.ReadLine(), out int rentalIdx) || rentalIdx < 1 || rentalIdx > activeRentals.Count)
                { Console.WriteLine("Невірний вибір."); break; }

                var completed = service.ReturnCar(activeRentals[rentalIdx - 1].Id);
                Console.WriteLine($"\n✓ Авто повернено! Загальна вартість: {completed.TotalCost} грн");
                break;

            case "5":
                var active = service.GetActiveRentals();
                if (!active.Any()) { Console.WriteLine("Немає активних оренд."); break; }
                Console.WriteLine("\nАктивні оренди:");
                foreach (var r in active)
                    Console.WriteLine($"  {r}");
                break;

            case "0":
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