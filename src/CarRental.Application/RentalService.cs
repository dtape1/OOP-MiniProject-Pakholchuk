using CarRental.Domain;
using CarRental.Domain.Interfaces;

namespace CarRental.Application;

public class RentalService
{
    private readonly ICarRepository _cars;
    private readonly IClientRepository _clients;
    private readonly IRentalRepository _rentals;

    public RentalService(ICarRepository cars, IClientRepository clients, IRentalRepository rentals)
    {
        _cars = cars;
        _clients = clients;
        _rentals = rentals;
    }

    public Rental RentCar(Guid clientId, Guid carId, DateTime startDate, DateTime endDate)
    {
        var car = _cars.GetById(carId)
            ?? throw new InvalidOperationException($"Автомобіль {carId} не знайдено");

        var client = _clients.GetById(clientId)
            ?? throw new InvalidOperationException($"Клієнта {clientId} не знайдено");

        if (!car.IsAvailable)
            throw new InvalidOperationException($"Автомобіль {car} вже орендований");

        var rental = new Rental(car, client, startDate, endDate);
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

    public List<Rental> GetActiveRentals() => _rentals.GetActive();

    public List<Car> GetAvailableCars() => _cars.GetAvailable();

    public List<Client> GetAllClients() => _clients.GetAll();
}