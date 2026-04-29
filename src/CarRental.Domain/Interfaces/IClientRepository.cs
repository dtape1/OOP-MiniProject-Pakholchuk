namespace CarRental.Domain.Interfaces;

public interface IClientRepository
{
    void Add(Client client);
    Client? GetById(Guid id);
    List<Client> GetAll();
}