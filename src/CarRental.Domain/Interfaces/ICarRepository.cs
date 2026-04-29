namespace CarRental.Domain.Interfaces;

public interface ICarRepository
{
    void Add(Car car);
    Car? GetById(Guid id);
    List<Car> GetAll();
    List<Car> GetAvailable();
}