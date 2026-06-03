using CarRental.Application;
using CarRental.Application.Pricing;
using CarRental.Console.Commands;
using CarRental.Domain;
using CarRental.Infrastructure;



var carRepo = new JsonCarRepository("data/cars.json");
var clientRepo = new JsonClientRepository("data/clients.json");
var rentalRepo = new JsonRentalRepository("data/rentals.json", carRepo, clientRepo);

await carRepo.LoadAsync();
await clientRepo.LoadAsync();
await rentalRepo.LoadAsync();

var service = new RentalService(carRepo, clientRepo, rentalRepo);
service.RegisterPricingStrategy(new StandardPricingStrategy());
service.RegisterPricingStrategy(new DiscountPricingStrategy(10));
service.RegisterPricingStrategy(new DiscountPricingStrategy(20));

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

async Task SaveAll() { await carRepo.SaveAsync(); await clientRepo.SaveAsync(); await rentalRepo.SaveAsync(); }

// Graceful shutdown
AppDomain.CurrentDomain.ProcessExit += async (s, e) =>
{
    Console.WriteLine("\n Збереження даних перед зупинкою...");
    await carRepo.SaveAsync();
    await clientRepo.SaveAsync();
    await rentalRepo.SaveAsync();
    Console.WriteLine("Дані збережено. До побачення!");
};

Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    Console.WriteLine("\n Отримано сигнал зупинки. Зберігаємо дані...");
    carRepo.SaveAsync().GetAwaiter().GetResult();
    clientRepo.SaveAsync().GetAwaiter().GetResult();
    rentalRepo.SaveAsync().GetAwaiter().GetResult();
    Console.WriteLine("Готово!");
    Environment.Exit(0);
};

var commands = new CarRental.Console.Commands.ICommand[]
{
    new ListCarsCommand(service),
    new ListClientsCommand(service),
    new RentCarCommand(service, SaveAll),
    new ReturnCarCommand(service, SaveAll),
    new CancelRentalCommand(service, SaveAll),
    new ActiveRentalsCommand(service),
    new AnalyticsCommand(service),
    new SaveDataCommand(SaveAll),
};

var app = new CarRental.Console.AppRunner(commands, SaveAll);
await app.RunAsync();