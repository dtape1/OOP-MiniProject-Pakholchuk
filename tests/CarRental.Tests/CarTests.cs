using CarRental.Domain;

namespace CarRental.Tests;

public class CarTests
{
    [Fact]
    public void Car_CreatedWithValidData_IsAvailable()
    {
        var car = new Car("Toyota", "Camry", 2021, 800);
        Assert.True(car.IsAvailable);
    }

    [Fact]
    public void Car_MakeUnavailable_SetsIsAvailableToFalse()
    {
        var car = new Car("Toyota", "Camry", 2021, 800);
        car.MakeUnavailable();
        Assert.False(car.IsAvailable);
    }

    [Fact]
    public void Car_MakeAvailable_SetsIsAvailableToTrue()
    {
        var car = new Car("Toyota", "Camry", 2021, 800);
        car.MakeUnavailable();
        car.MakeAvailable();
        Assert.True(car.IsAvailable);
    }

    [Fact]
    public void Car_WithEmptyBrand_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Car("", "Camry", 2021, 800));
    }

    [Fact]
    public void Car_WithNegativePrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Car("Toyota", "Camry", 2021, -100));
    }
}