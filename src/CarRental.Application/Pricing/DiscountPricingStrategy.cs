using CarRental.Domain.Interfaces;

namespace CarRental.Application.Pricing;

public class DiscountPricingStrategy : IPricingStrategy
{
    private readonly decimal _discountPercent;

    public DiscountPricingStrategy(decimal discountPercent = 10)
    {
        if (discountPercent < 0 || discountPercent > 100)
            throw new ArgumentException("Знижка повинна бути від 0 до 100%");
        _discountPercent = discountPercent;
    }

    public string Name => $"Зі знижкою {_discountPercent}%";

    public decimal Calculate(decimal pricePerDay, int days)
        => pricePerDay * days * (1 - _discountPercent / 100);
}