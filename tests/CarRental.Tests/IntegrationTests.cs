using CarRental.Application;
using CarRental.Application.Pricing;
using CarRental.Domain;
using CarRental.Infrastructure;

namespace CarRental.Tests;

public class IntegrationTests : IDisposable
{
    private readonly string _tempDir;

    public IntegrationTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose() => Directory.Delete(_tempDir, true);

    private (JsonCarRepository, JsonClientRepository, JsonRentalRepository, RentalService) CreateRepos()
    {
        var carRepo = new JsonCarRepository(Path.Combine(_tempDir, "cars.json"));
        var clientRepo = new JsonClientRepository(Path.Combine(_tempDir, "clients.json"));
        var rentalRepo = new JsonRentalRepository(Path.Combine(_tempDir, "rentals.json"), carRepo, clientRepo);
        var service = new RentalService(carRepo, clientRepo, rentalRepo);
        service.RegisterPricingStrategy(new StandardPricingStrategy());
        return (carRepo, clientRepo, rentalRepo, service);
    }

    [Fact]
    public async Task SaveAndReload_Cars_PreservesData()
    {
        var (carRepo, _, _, service) = CreateRepos();
        service.AddCar(new Car("Toyota", "Camry", 2021, 800));
        await carRepo.SaveAsync();

        var carRepo2 = new JsonCarRepository(Path.Combine(_tempDir, "cars.json"));
        await carRepo2.LoadAsync();
        Assert.Single(carRepo2.GetAll());
        Assert.Equal("Toyota", carRepo2.GetAll()[0].Brand);
    }

    [Fact]
    public async Task SaveAndReload_Clients_PreservesData()
    {
        var (_, clientRepo, _, service) = CreateRepos();
        service.AddClient(new Client("Тест Юзер", "t@t.com", "+380501234567"));
        await clientRepo.SaveAsync();

        var clientRepo2 = new JsonClientRepository(Path.Combine(_tempDir, "clients.json"));
        await clientRepo2.LoadAsync();
        Assert.Single(clientRepo2.GetAll());
        Assert.Equal("Тест Юзер", clientRepo2.GetAll()[0].FullName);
    }

    [Fact]
    public async Task SaveAndReload_Rental_PreservesStatus()
    {
        var (carRepo, clientRepo, rentalRepo, service) = CreateRepos();
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        service.AddCar(car); service.AddClient(client);
        var rental = service.RentCar(client.Id, car.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 4));
        service.ReturnCar(rental.Id);
        await carRepo.SaveAsync(); await clientRepo.SaveAsync(); await rentalRepo.SaveAsync();

        var carRepo2 = new JsonCarRepository(Path.Combine(_tempDir, "cars.json"));
        var clientRepo2 = new JsonClientRepository(Path.Combine(_tempDir, "clients.json"));
        var rentalRepo2 = new JsonRentalRepository(Path.Combine(_tempDir, "rentals.json"), carRepo2, clientRepo2);
        await carRepo2.LoadAsync(); await clientRepo2.LoadAsync(); await rentalRepo2.LoadAsync();

        var rentals = rentalRepo2.GetAll();
        Assert.Single(rentals);
        Assert.Equal(RentalStatus.Completed, rentals[0].Status);
    }

    [Fact]
    public async Task SaveAndReload_CarAvailability_PreservesState()
    {
        var (carRepo, clientRepo, rentalRepo, service) = CreateRepos();
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        service.AddCar(car); service.AddClient(client);
        service.RentCar(client.Id, car.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 4));
        await carRepo.SaveAsync();

        var carRepo2 = new JsonCarRepository(Path.Combine(_tempDir, "cars.json"));
        await carRepo2.LoadAsync();
        Assert.False(carRepo2.GetAll()[0].IsAvailable);
    }

    [Fact]
    public async Task LoadAsync_MissingFile_ReturnsEmpty()
    {
        var carRepo = new JsonCarRepository(Path.Combine(_tempDir, "nonexistent.json"));
        await carRepo.LoadAsync();
        Assert.Empty(carRepo.GetAll());
    }

    [Fact]
    public async Task LoadAsync_CorruptedJson_ReturnsEmpty()
    {
        var path = Path.Combine(_tempDir, "cars.json");
        await File.WriteAllTextAsync(path, "{ це не json !!!");
        var carRepo = new JsonCarRepository(path);
        await carRepo.LoadAsync();
        Assert.Empty(carRepo.GetAll());
    }

    [Fact]
    public async Task FullCycle_RentReturnReload_Works()
    {
        var (carRepo, clientRepo, rentalRepo, service) = CreateRepos();
        var car = new Car("BMW", "X5", 2022, 1500);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        service.AddCar(car); service.AddClient(client);

        var rental = service.RentCar(client.Id, car.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 6));
        Assert.Equal(RentalStatus.Active, rental.Status);
        service.ReturnCar(rental.Id);
        Assert.Equal(RentalStatus.Completed, rental.Status);

        await carRepo.SaveAsync(); await clientRepo.SaveAsync(); await rentalRepo.SaveAsync();

        var carRepo2 = new JsonCarRepository(Path.Combine(_tempDir, "cars.json"));
        var clientRepo2 = new JsonClientRepository(Path.Combine(_tempDir, "clients.json"));
        var rentalRepo2 = new JsonRentalRepository(Path.Combine(_tempDir, "rentals.json"), carRepo2, clientRepo2);
        await carRepo2.LoadAsync(); await clientRepo2.LoadAsync(); await rentalRepo2.LoadAsync();

        Assert.True(carRepo2.GetAll()[0].IsAvailable);
        Assert.Equal(7500, rentalRepo2.GetAll()[0].TotalCost);
    }

    [Fact]
    public async Task MultipleSequentialSaves_PreservesAllData()
    {
        var (carRepo, clientRepo, rentalRepo, service) = CreateRepos();
        var client = new Client("Тест", "t@t.com", "+380501234567");
        service.AddClient(client);

        var car1 = new Car("Toyota", "Camry", 2021, 800);
        var car2 = new Car("BMW", "X5", 2022, 1500);
        service.AddCar(car1); service.AddCar(car2);

        var rental1 = service.RentCar(client.Id, car1.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));
        await carRepo.SaveAsync(); await clientRepo.SaveAsync(); await rentalRepo.SaveAsync();

        service.ReturnCar(rental1.Id);
        service.RentCar(client.Id, car2.Id, new DateTime(2026, 5, 5), new DateTime(2026, 5, 8));
        await carRepo.SaveAsync(); await rentalRepo.SaveAsync();

        var carRepo2 = new JsonCarRepository(Path.Combine(_tempDir, "cars.json"));
        var clientRepo2 = new JsonClientRepository(Path.Combine(_tempDir, "clients.json"));
        var rentalRepo2 = new JsonRentalRepository(Path.Combine(_tempDir, "rentals.json"), carRepo2, clientRepo2);
        await carRepo2.LoadAsync(); await clientRepo2.LoadAsync(); await rentalRepo2.LoadAsync();

        Assert.Equal(2, rentalRepo2.GetAll().Count);
    }
}