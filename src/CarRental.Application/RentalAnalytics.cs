using CarRental.Domain;

namespace CarRental.Application;

public class RentalAnalytics
{
    private readonly RentalService _service;

    public RentalAnalytics(RentalService service)
    {
        _service = service;
    }

    /// <summary>Дохід по кожній стратегії тарифу</summary>
    public Dictionary<string, decimal> GetRevenueByPricingStrategy()
        => _service
            .FilterRentals(r => r.Status == RentalStatus.Completed)
            .GroupBy(r => r.PricingStrategyName)
            .ToDictionary(g => g.Key, g => g.Sum(r => r.TotalCost));

    /// <summary>Топ клієнтів за кількістю оренд</summary>
    public List<(string ClientName, int RentalCount)> GetTopClients(int top = 3)
        => _service
            .FilterRentals(_ => true)
            .GroupBy(r => r.Client.FullName)
            .Select(g => (ClientName: g.Key, RentalCount: g.Count()))
            .OrderByDescending(x => x.RentalCount)
            .Take(top)
            .ToList();

    /// <summary>Відсоток використання автопарку</summary>
    public double GetUtilizationRate()
    {
        var allCars = _service.FilterCars(_ => true);
        if (!allCars.Any()) return 0;
        var unavailable = _service.FilterCars(c => !c.IsAvailable).Count;
        return Math.Round((double)unavailable / allCars.Count * 100, 1);
    }

    /// <summary>Авто дорожчі за вказану ціну</summary>
    public List<Car> GetCarsExpensiveThan(decimal price)
        => _service.FilterCars(c => c.PricePerDay > price);

    /// <summary>Оренди в діапазоні дат</summary>
    public List<Rental> GetRentalsInDateRange(DateTime from, DateTime to)
        => _service.FilterRentals(r => r.StartDate >= from && r.EndDate <= to);
}