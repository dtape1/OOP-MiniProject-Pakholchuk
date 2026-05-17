using CarRental.Application;

namespace CarRental.Console.Commands;

public class CancelRentalCommand : ICommand
{
    private readonly RentalService _service;
    private readonly Func<Task> _saveAsync;
    public string Key => "5";
    public string Description => "Скасувати оренду";

    public CancelRentalCommand(RentalService service, Func<Task> saveAsync)
    {
        _service = service;
        _saveAsync = saveAsync;
    }

    public async Task ExecuteAsync()
    {
        var active = _service.GetActiveRentals();
        if (!active.Any()) { System.Console.WriteLine("Немає активних оренд."); return; }
        System.Console.WriteLine("\nАктивні оренди:");
        for (int i = 0; i < active.Count; i++)
            System.Console.WriteLine($"  {i + 1}. {active[i]}");
        System.Console.Write("Оберіть номер для скасування: ");
        if (!int.TryParse(System.Console.ReadLine(), out int idx) || idx < 1 || idx > active.Count)
        { System.Console.WriteLine("Невірний вибір."); return; }
        var cancelled = _service.CancelRental(active[idx - 1].Id);
        await _saveAsync();
        System.Console.WriteLine($"\n✓ Оренду скасовано: {cancelled}");
    }
}