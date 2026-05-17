using CarRental.Application;
using CarRental.Application.Pricing;
using CarRental.Infrastructure;

namespace CarRental.Tests;

public static class TestHelpers
{
    public static RentalService CreateService()
    {
        var service = new RentalService(
            new InMemoryCarRepository(),
            new InMemoryClientRepository(),
            new InMemoryRentalRepository());
        service.RegisterPricingStrategy(new StandardPricingStrategy());
        service.RegisterPricingStrategy(new DiscountPricingStrategy(10));
        return service;
    }
}