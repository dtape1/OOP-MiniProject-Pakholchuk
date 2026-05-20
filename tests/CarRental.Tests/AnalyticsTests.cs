using CarRental.Application;
using CarRental.Domain;
using CarRental.Infrastructure;

namespace CarRental.Tests;

public class AnalyticsTests
{
    private (RentalService service, RentalAnalytics analytics) CreateSetup()
    {
        var service = TestHelpers.CreateService();
        var analytics = new RentalAnalytics(service);
        return (service, analytics);
    }

    [Fact]
    public void FilterCars_ByAvailability_ReturnsOnlyAvailable()
    {
        var (service, analytics) = CreateSetup();
        service.AddCar(new Car("Toyota", "Camry", 2021, 800));
        service.AddCar(new Car("BMW", "X5", 2022, 1500));
        var client = new Client("Тест", "t@t.com", "+380501234567");
        service.AddClient(client);
        var cars = service.GetAvailableCars();
        service.RentCar(client.Id, cars[0].Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));

        var result = service.FilterCars(c => c.IsAvailable);
        Assert.Single(result);
    }

    [Fact]
    public void FilterCars_ByPrice_ReturnsCorrect()
    {
        var (service, _) = CreateSetup();
        service.AddCar(new Car("Toyota", "Camry", 2021, 800));
        service.AddCar(new Car("BMW", "X5", 2022, 1500));
        service.AddCar(new Car("Renault", "Logan", 2020, 500));

        var result = service.FilterCars(c => c.PricePerDay > 700);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void FilterRentals_ByStatus_ReturnsOnlyActive()
    {
        var (service, _) = CreateSetup();
        var car1 = new Car("Toyota", "Camry", 2021, 800);
        var car2 = new Car("BMW", "X5", 2022, 1500);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        service.AddCar(car1); service.AddCar(car2);
        service.AddClient(client);

        var r1 = service.RentCar(client.Id, car1.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));
        service.RentCar(client.Id, car2.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));
        service.ReturnCar(r1.Id);

        var result = service.FilterRentals(r => r.Status == RentalStatus.Active);
        Assert.Single(result);
    }

    [Theory]
    [InlineData(700, 2)]
    [InlineData(800, 1)]
    [InlineData(1500, 0)]
    public void GetCarsExpensiveThan_Theory_ReturnsCorrectCount(decimal price, int expected)
    {
        var (_, analytics) = CreateSetup();
        var (service, _) = CreateSetup();
        service.AddCar(new Car("Toyota", "Camry", 2021, 800));
        service.AddCar(new Car("BMW", "X5", 2022, 1500));
        service.AddCar(new Car("Renault", "Logan", 2020, 500));
        var a = new RentalAnalytics(service);

        var result = a.GetCarsExpensiveThan(price);
        Assert.Equal(expected, result.Count);
    }

    [Fact]
    public void GetUtilizationRate_AllAvailable_ReturnsZero()
    {
        var (service, analytics) = CreateSetup();
        service.AddCar(new Car("Toyota", "Camry", 2021, 800));
        Assert.Equal(0, analytics.GetUtilizationRate());
    }

    [Fact]
    public void GetUtilizationRate_HalfRented_Returns50()
    {
        var (service, analytics) = CreateSetup();
        var car1 = new Car("Toyota", "Camry", 2021, 800);
        var car2 = new Car("BMW", "X5", 2022, 1500);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        service.AddCar(car1); service.AddCar(car2);
        service.AddClient(client);
        service.RentCar(client.Id, car1.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));

        Assert.Equal(50, analytics.GetUtilizationRate());
    }

    [Fact]
    public void GetRevenueByPricingStrategy_ReturnsCorrect()
    {
        var (service, analytics) = CreateSetup();
        var car = new Car("Toyota", "Camry", 2021, 1000);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        service.AddCar(car); service.AddClient(client);

        var rental = service.RentCar(client.Id, car.Id,
            new DateTime(2026, 5, 1), new DateTime(2026, 5, 4), "Стандартний");
        service.ReturnCar(rental.Id);

        var result = analytics.GetRevenueByPricingStrategy();
        Assert.True(result.ContainsKey("Стандартний"));
        Assert.Equal(3000, result["Стандартний"]);
    }

    [Fact]
    public void GetRentalsInDateRange_ReturnsCorrect()
    {
        var (service, analytics) = CreateSetup();
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        service.AddCar(car); service.AddClient(client);
        service.RentCar(client.Id, car.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 5));

        var result = analytics.GetRentalsInDateRange(
            new DateTime(2026, 4, 1), new DateTime(2026, 6, 1));
        Assert.Single(result);
    }

    [Fact]
    public void GetTopClients_ReturnsCorrectOrder()
    {
        var (service, analytics) = CreateSetup();
        var car1 = new Car("Toyota", "Camry", 2021, 800);
        var car2 = new Car("BMW", "X5", 2022, 1500);
        var client1 = new Client("Клієнт А", "a@t.com", "+380501111111");
        var client2 = new Client("Клієнт Б", "b@t.com", "+380502222222");
        service.AddCar(car1); service.AddCar(car2);
        service.AddClient(client1); service.AddClient(client2);

        var r1 = service.RentCar(client1.Id, car1.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));
        service.ReturnCar(r1.Id);
        service.RentCar(client1.Id, car1.Id, new DateTime(2026, 5, 4), new DateTime(2026, 5, 6));
        service.RentCar(client2.Id, car2.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));

        var result = analytics.GetTopClients(2);
        Assert.Equal("Клієнт А", result[0].ClientName);
        Assert.Equal(2, result[0].RentalCount);
    }
}