using CarRental.Application;

namespace CarRental.Console.Commands;

public class ListClientsCommand : ICommand
{
    private readonly RentalService _service;
    public string Key => "2";
    public string Description => "Переглянути клієнтів";

    public ListClientsCommand(RentalService service) => _service = service;

    public Task ExecuteAsync()
    {
        var clients = _service.GetAllClients();
        if (!clients.Any()) { System.Console.WriteLine("Немає клієнтів."); return Task.CompletedTask; }
        System.Console.WriteLine("\nКлієнти:");
        for (int i = 0; i < clients.Count; i++)
            System.Console.WriteLine($"  {i + 1}. [{clients[i].Id.ToString()[..8]}] {clients[i]}");
        return Task.CompletedTask;
    }
}