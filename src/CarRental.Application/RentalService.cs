using CarRental.Domain;
using CarRental.Domain.Interfaces;

namespace CarRental.Application;

public class RentalService
{
    private readonly ICarRepository _cars;
    private readonly IClientRepository _clients;
    private readonly IRentalRepository _rentals;
    private readonly Dictionary<string, IPricingStrategy> _pricingStrategies;

    public RentalService(ICarRepository cars, IClientRepository clients, IRentalRepository rentals)
    {
        _cars = cars;
        _clients = clients;
        _rentals = rentals;
        _pricingStrategies = new Dictionary<string, IPricingStrategy>();
    }

    public void RegisterPricingStrategy(IPricingStrategy strategy)
        => _pricingStrategies[strategy.Name] = strategy;

    public List<string> GetAvailableStrategies()
        => _pricingStrategies.Keys.ToList();

    public Rental RentCar(Guid clientId, Guid carId, DateTime startDate, DateTime endDate, string strategyName = "Стандартний")
    {
        var car = _cars.GetById(carId)
            ?? throw new InvalidOperationException($"Автомобіль {carId} не знайдено");

        var client = _clients.GetById(clientId)
            ?? throw new InvalidOperationException($"Клієнта {clientId} не знайдено");

        if (!car.IsAvailable)
            throw new InvalidOperationException($"Автомобіль {car} вже орендований");

        var rental = new Rental(car, client, startDate, endDate, strategyName);

        if (_pricingStrategies.TryGetValue(strategyName, out var strategy))
        {
            int days = (int)(endDate - startDate).TotalDays;
            rental.SetTotalCost(strategy.Calculate(car.PricePerDay, days));
        }

        car.MakeUnavailable();
        _rentals.Add(rental);
        return rental;
    }

    public Rental ReturnCar(Guid rentalId)
    {
        var rental = _rentals.GetById(rentalId)
            ?? throw new InvalidOperationException($"Оренду {rentalId} не знайдено");
        rental.Complete();
        rental.Car.MakeAvailable();
        return rental;
    }

    public Rental CancelRental(Guid rentalId)
    {
        var rental = _rentals.GetById(rentalId)
            ?? throw new InvalidOperationException($"Оренду {rentalId} не знайдено");
        rental.Cancel();
        rental.Car.MakeAvailable();
        return rental;
    }

    // LINQ запити
    public List<Car> GetAvailableCars() => _cars.GetAvailable();

    public List<Car> GetCarsSortedByPrice() =>
        _cars.GetAll().OrderBy(c => c.PricePerDay).ToList();

    public List<Car> GetCarsInPriceRange(decimal min, decimal max) =>
        _cars.GetAll().Where(c => c.PricePerDay >= min && c.PricePerDay <= max).ToList();

    public List<Rental> GetActiveRentals() => _rentals.GetActive();

    public List<Rental> GetRentalsByClient(Guid clientId) =>
        _rentals.GetAll().Where(r => r.Client.Id == clientId).ToList();

    public decimal GetTotalRevenue() =>
        _rentals.GetAll().Where(r => r.Status == RentalStatus.Completed).Sum(r => r.TotalCost);

    public List<Client> GetAllClients() => _clients.GetAll();

    public void AddCar(Car car) => _cars.Add(car);
    public void AddClient(Client client) => _clients.Add(client);
}