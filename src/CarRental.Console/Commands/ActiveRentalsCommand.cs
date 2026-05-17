using CarRental.Application;

namespace CarRental.Console.Commands;

public class ActiveRentalsCommand : ICommand
{
    private readonly RentalService _service;
    public string Key => "6";
    public string Description => "Активні оренди";

    public ActiveRentalsCommand(RentalService service) => _service = service;

    public Task ExecuteAsync()
    {
        var active = _service.GetActiveRentals();
        if (!active.Any()) { System.Console.WriteLine("Немає активних оренд."); return Task.CompletedTask; }
        System.Console.WriteLine("\nАктивні оренди:");
        foreach (var r in active)
            System.Console.WriteLine($"  {r}");
        return Task.CompletedTask;
    }
}