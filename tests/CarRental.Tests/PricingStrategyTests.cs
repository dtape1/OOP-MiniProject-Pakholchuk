using CarRental.Application.Pricing;

namespace CarRental.Tests;

public class PricingStrategyTests
{
    [Fact]
    public void StandardPricing_CalculatesCorrectly()
    {
        var strategy = new StandardPricingStrategy();
        Assert.Equal(2400, strategy.Calculate(800, 3));
    }

    [Fact]
    public void DiscountPricing_10Percent_CalculatesCorrectly()
    {
        var strategy = new DiscountPricingStrategy(10);
        Assert.Equal(2160, strategy.Calculate(800, 3));
    }

    [Fact]
    public void DiscountPricing_InvalidPercent_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new DiscountPricingStrategy(150));
    }

    [Fact]
    public void DiscountPricing_ZeroPercent_SameAsStandard()
    {
        var discount = new DiscountPricingStrategy(0);
        var standard = new StandardPricingStrategy();
        Assert.Equal(standard.Calculate(1000, 5), discount.Calculate(1000, 5));
    }
}