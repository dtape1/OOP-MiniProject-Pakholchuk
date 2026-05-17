using CarRental.Application;

namespace CarRental.Console.Commands;

public class AnalyticsCommand : ICommand
{
    private readonly RentalService _service;
    public string Key => "7";
    public string Description => "Аналітика";

    public AnalyticsCommand(RentalService service) => _service = service;

    public Task ExecuteAsync()
    {
        System.Console.WriteLine("\n=== Аналітика ===");
        System.Console.WriteLine($"Загальний дохід: {_service.GetTotalRevenue()} грн");
        System.Console.WriteLine($"Доступних авто: {_service.GetAvailableCars().Count}");
        System.Console.WriteLine($"Активних оренд: {_service.GetActiveRentals().Count}");
        System.Console.Write("\nФільтр авто за ціною — мін (грн): ");
        if (decimal.TryParse(System.Console.ReadLine(), out decimal min))
        {
            System.Console.Write("Макс (грн): ");
            if (decimal.TryParse(System.Console.ReadLine(), out decimal max))
            {
                var filtered = _service.GetCarsInPriceRange(min, max);
                System.Console.WriteLine($"\nАвто від {min} до {max} грн/день:");
                foreach (var c in filtered)
                    System.Console.WriteLine($"  {c}");
            }
        }
        return Task.CompletedTask;
    }
}