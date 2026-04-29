using CarRental.Domain;
using CarRental.Domain.Interfaces;

namespace CarRental.Infrastructure;

public class InMemoryCarRepository : ICarRepository
{
    private readonly List<Car> _cars = new();

    public void Add(Car car) => _cars.Add(car);

    public Car? GetById(Guid id) => _cars.FirstOrDefault(c => c.Id == id);

    public List<Car> GetAll() => _cars.ToList();

    public List<Car> GetAvailable() => _cars.Where(c => c.IsAvailable).ToList();
}