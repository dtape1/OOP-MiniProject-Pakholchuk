using CarRental.Application;
using CarRental.Application.Pricing;
using CarRental.Domain;
using CarRental.Infrastructure;

namespace CarRental.Tests;

public class RentalServiceTests
{
    private RentalService CreateService()
    {
        var service = new RentalService(
            new InMemoryCarRepository(),
            new InMemoryClientRepository(),
            new InMemoryRentalRepository());
        service.RegisterPricingStrategy(new StandardPricingStrategy());
        service.RegisterPricingStrategy(new DiscountPricingStrategy(10));
        return service;
    }

    [Fact]
    public void RentCar_ValidData_CreatesRental()
    {
        var service = CreateService();
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест Юзер", "test@email.com", "+380501234567");
        service.AddCar(car);
        service.AddClient(client);

        var rental = service.RentCar(client.Id, car.Id,
            new DateTime(2026, 5, 1), new DateTime(2026, 5, 4));

        Assert.NotNull(rental);
        Assert.Equal(RentalStatus.Active, rental.Status);
    }

    [Fact]
    public void RentCar_MakesCarUnavailable()
    {
        var service = CreateService();
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест Юзер", "test@email.com", "+380501234567");
        service.AddCar(car);
        service.AddClient(client);

        service.RentCar(client.Id, car.Id,
            new DateTime(2026, 5, 1), new DateTime(2026, 5, 4));

        Assert.Empty(service.GetAvailableCars());
    }

    [Fact]
    public void RentCar_UnavailableCar_ThrowsException()
    {
        var service = CreateService();
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест Юзер", "test@email.com", "+380501234567");
        service.AddCar(car);
        service.AddClient(client);

        service.RentCar(client.Id, car.Id,
            new DateTime(2026, 5, 1), new DateTime(2026, 5, 4));

        Assert.Throws<InvalidOperationException>(() =>
            service.RentCar(client.Id, car.Id,
                new DateTime(2026, 5, 1), new DateTime(2026, 5, 4)));
    }

    [Fact]
    public void ReturnCar_CompletesRentalAndMakesCarAvailable()
    {
        var service = CreateService();
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест Юзер", "test@email.com", "+380501234567");
        service.AddCar(car);
        service.AddClient(client);

        var rental = service.RentCar(client.Id, car.Id,
            new DateTime(2026, 5, 1), new DateTime(2026, 5, 4));
        service.ReturnCar(rental.Id);

        Assert.Equal(RentalStatus.Completed, rental.Status);
        Assert.True(car.IsAvailable);
    }

    [Fact]
    public void CancelRental_CancelsAndMakesCarAvailable()
    {
        var service = CreateService();
        var car = new Car("BMW", "X5", 2022, 1500);
        var client = new Client("Тест Юзер", "test@email.com", "+380501234567");
        service.AddCar(car);
        service.AddClient(client);

        var rental = service.RentCar(client.Id, car.Id,
            new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));
        service.CancelRental(rental.Id);

        Assert.Equal(RentalStatus.Cancelled, rental.Status);
        Assert.True(car.IsAvailable);
    }

    [Fact]
    public void GetTotalRevenue_OnlyCountsCompleted()
    {
        var service = CreateService();
        var car1 = new Car("Toyota", "Camry", 2021, 800);
        var car2 = new Car("BMW", "X5", 2022, 1500);
        var client = new Client("Тест Юзер", "test@email.com", "+380501234567");
        service.AddCar(car1);
        service.AddCar(car2);
        service.AddClient(client);

        var r1 = service.RentCar(client.Id, car1.Id,
            new DateTime(2026, 5, 1), new DateTime(2026, 5, 4));
        service.ReturnCar(r1.Id); // completed

        service.RentCar(client.Id, car2.Id,
            new DateTime(2026, 5, 1), new DateTime(2026, 5, 3)); // active

        Assert.Equal(r1.TotalCost, service.GetTotalRevenue());
    }

    [Fact]
    public void GetCarsInPriceRange_ReturnsCorrectCars()
    {
        var service = CreateService();
        service.AddCar(new Car("Toyota", "Camry", 2021, 800));
        service.AddCar(new Car("BMW", "X5", 2022, 1500));
        service.AddCar(new Car("Renault", "Logan", 2020, 500));

        var result = service.GetCarsInPriceRange(600, 1000);

        Assert.Single(result);
        Assert.Equal("Toyota", result[0].Brand);
    }

    [Fact]
    public void RentCar_WithDiscountStrategy_AppliesDiscount()
    {
        var service = CreateService();
        var car = new Car("Toyota", "Camry", 2021, 1000);
        var client = new Client("Тест Юзер", "test@email.com", "+380501234567");
        service.AddCar(car);
        service.AddClient(client);

        var rental = service.RentCar(client.Id, car.Id,
            new DateTime(2026, 5, 1), new DateTime(2026, 5, 6), "Зі знижкою 10%");

        Assert.Equal(4500, rental.TotalCost); // 5 днів * 1000 * 0.9
    }
}