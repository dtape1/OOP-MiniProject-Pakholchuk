using CarRental.Domain;
using CarRental.Domain.Interfaces;

namespace CarRental.Infrastructure;

public class InMemoryClientRepository : IClientRepository
{
    private readonly List<Client> _clients = new();

    public void Add(Client client) => _clients.Add(client);

    public Client? GetById(Guid id) => _clients.FirstOrDefault(c => c.Id == id);

    public List<Client> GetAll() => _clients.ToList();
}