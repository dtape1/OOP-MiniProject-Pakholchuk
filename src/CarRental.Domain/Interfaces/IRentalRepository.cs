namespace CarRental.Domain.Interfaces;

public interface IRentalRepository
{
    void Add(Rental rental);
    Rental? GetById(Guid id);
    List<Rental> GetAll();
    List<Rental> GetActive();
}