using CarRental.Domain;

namespace CarRental.Tests;

public class CarRentalTheoryTests
{
    [Theory]
    [InlineData("", "Camry", 2021, 800)]
    [InlineData("Toyota", "", 2021, 800)]
    [InlineData("Toyota", "Camry", 1980, 800)]
    [InlineData("Toyota", "Camry", 2021, -100)]
    [InlineData("Toyota", "Camry", 2021, 0)]
    public void Car_InvalidData_ThrowsArgumentException(string brand, string model, int year, decimal price)
    {
        Assert.Throws<ArgumentException>(() => new Car(brand, model, year, price));
    }

    [Theory]
    [InlineData("", "test@email.com", "+380501234567")]
    [InlineData("Іван", "", "+380501234567")]
    [InlineData("Іван", "test@email.com", "")]
    public void Client_InvalidData_ThrowsArgumentException(string name, string email, string phone)
    {
        Assert.Throws<ArgumentException>(() => new Client(name, email, phone));
    }

    [Theory]
    [InlineData(1, 800)]
    [InlineData(3, 2400)]
    [InlineData(7, 5600)]
    [InlineData(30, 24000)]
    public void Rental_CalculateCost_CorrectForDifferentDurations(int days, decimal expected)
    {
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        var start = new DateTime(2026, 5, 1);
        var end = start.AddDays(days);
        var rental = new Rental(car, client, start, end);
        Assert.Equal(expected, rental.TotalCost);
    }

    [Fact]
    public void Rental_CompleteAlreadyCompleted_ThrowsException()
    {
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        var rental = new Rental(car, client, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));
        rental.Complete();
        Assert.Throws<InvalidOperationException>(() => rental.Complete());
    }

    [Fact]
    public void Rental_CancelAlreadyCancelled_ThrowsException()
    {
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        var rental = new Rental(car, client, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));
        rental.Cancel();
        Assert.Throws<InvalidOperationException>(() => rental.Cancel());
    }

    [Fact]
    public void Rental_CancelAfterComplete_ThrowsException()
    {
        var car = new Car("Toyota", "Camry", 2021, 800);
        var client = new Client("Тест", "t@t.com", "+380501234567");
        var rental = new Rental(car, client, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));
        rental.Complete();
        Assert.Throws<InvalidOperationException>(() => rental.Cancel());
    }

    [Fact]
    public void Rental_NullCar_ThrowsArgumentNullException()
    {
        var client = new Client("Тест", "t@t.com", "+380501234567");
        Assert.Throws<ArgumentNullException>(() =>
            new Rental(null!, client, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3)));
    }

    [Fact]
    public void Rental_NullClient_ThrowsArgumentNullException()
    {
        var car = new Car("Toyota", "Camry", 2021, 800);
        Assert.Throws<ArgumentNullException>(() =>
            new Rental(car, null!, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3)));
    }

    [Fact]
    public void RentalService_ReturnNonExistentRental_ThrowsException()
    {
        var service = TestHelpers.CreateService();
        Assert.Throws<InvalidOperationException>(() => service.ReturnCar(Guid.NewGuid()));
    }

    [Fact]
    public void RentalService_CancelNonExistentRental_ThrowsException()
    {
        var service = TestHelpers.CreateService();
        Assert.Throws<InvalidOperationException>(() => service.CancelRental(Guid.NewGuid()));
    }

    [Fact]
    public void RentalService_GetRentalsByClient_ReturnsCorrect()
    {
        var service = TestHelpers.CreateService();
        var car1 = new Car("Toyota", "Camry", 2021, 800);
        var car2 = new Car("BMW", "X5", 2022, 1500);
        var client1 = new Client("Клієнт 1", "c1@t.com", "+380501111111");
        var client2 = new Client("Клієнт 2", "c2@t.com", "+380502222222");
        service.AddCar(car1); service.AddCar(car2);
        service.AddClient(client1); service.AddClient(client2);

        service.RentCar(client1.Id, car1.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));
        service.RentCar(client2.Id, car2.Id, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3));

        var result = service.GetRentalsByClient(client1.Id);
        Assert.Single(result);
        Assert.Equal(client1.Id, result[0].Client.Id);
    }
}