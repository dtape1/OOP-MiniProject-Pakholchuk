using CarRental.Domain;

namespace CarRental.Tests;

public class RentalTests
{
    private Car _car = new Car("BMW", "X5", 2022, 1500);
    private Client _client = new Client("Іван Мельник", "ivan@email.com", "+380501234567");

    [Fact]
    public void Rental_CalculateCost_ReturnsCorrectAmount()
    {
        var start = new DateTime(2026, 5, 1);
        var end = new DateTime(2026, 5, 5);
        var rental = new Rental(_car, _client, start, end);
        Assert.Equal(6000, rental.TotalCost); // 4 дні * 1500
    }

    [Fact]
    public void Rental_WithEndBeforeStart_ThrowsArgumentException()
    {
        var start = new DateTime(2026, 5, 5);
        var end = new DateTime(2026, 5, 1);
        Assert.Throws<ArgumentException>(() => new Rental(_car, _client, start, end));
    }

    [Fact]
    public void Rental_Complete_ChangesStatusToCompleted()
    {
        var rental = new Rental(_car, _client,
            new DateTime(2026, 5, 1),
            new DateTime(2026, 5, 3));
        rental.Complete();
        Assert.Equal(RentalStatus.Completed, rental.Status);
    }

    [Fact]
    public void Rental_Cancel_ChangesStatusToCancelled()
    {
        var rental = new Rental(_car, _client,
            new DateTime(2026, 5, 1),
            new DateTime(2026, 5, 3));
        rental.Cancel();
        Assert.Equal(RentalStatus.Cancelled, rental.Status);
    }

    [Fact]
    public void Rental_WithUnavailableCar_ThrowsInvalidOperationException()
    {
        _car.MakeUnavailable();
        Assert.Throws<InvalidOperationException>(() =>
            new Rental(_car, _client,
                new DateTime(2026, 5, 1),
                new DateTime(2026, 5, 3)));
    }
}