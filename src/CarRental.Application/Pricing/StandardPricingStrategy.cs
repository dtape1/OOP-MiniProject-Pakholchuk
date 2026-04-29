using CarRental.Domain.Interfaces;

namespace CarRental.Application.Pricing;

public class StandardPricingStrategy : IPricingStrategy
{
    public string Name => "Стандартний";

    public decimal Calculate(decimal pricePerDay, int days)
        => pricePerDay * days;
}