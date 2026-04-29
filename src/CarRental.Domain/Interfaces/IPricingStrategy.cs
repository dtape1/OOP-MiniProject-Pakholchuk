namespace CarRental.Domain.Interfaces;

public interface IPricingStrategy
{
    decimal Calculate(decimal pricePerDay, int days);
    string Name { get; }
}