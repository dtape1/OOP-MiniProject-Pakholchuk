using CarRental.Domain;
using CarRental.Domain.Interfaces;

namespace CarRental.Infrastructure;

public class InMemoryRentalRepository : IRentalRepository
{
    private readonly List<Rental> _rentals = new();

    public void Add(Rental rental) => _rentals.Add(rental);

    public Rental? GetById(Guid id) => _rentals.FirstOrDefault(r => r.Id == id);

    public List<Rental> GetAll() => _rentals.ToList();

    public List<Rental> GetActive() => _rentals.Where(r => r.Status == RentalStatus.Active).ToList();
}