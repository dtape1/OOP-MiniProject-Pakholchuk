using CarRental.Application;

namespace CarRental.Console.Commands;

public class ListCarsCommand : ICommand
{
    private readonly RentalService _service;
    public string Key => "1";
    public string Description => "Переглянути всі авто";

    public ListCarsCommand(RentalService service) => _service = service;

    public Task ExecuteAsync()
    {
        var cars = _service.GetCarsSortedByPrice();
        if (!cars.Any()) { System.Console.WriteLine("Немає авто."); return Task.CompletedTask; }
        System.Console.WriteLine("\nАвтомобілі (за ціною):");
        for (int i = 0; i < cars.Count; i++)
            System.Console.WriteLine($"  {i + 1}. [{cars[i].Id.ToString()[..8]}] {cars[i]} {(cars[i].IsAvailable ? "✓" : "✗ зайнятий")}");
        return Task.CompletedTask;
    }
}