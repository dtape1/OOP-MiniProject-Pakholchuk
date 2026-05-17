using CarRental.Application;

namespace CarRental.Console.Commands;

public class RentCarCommand : ICommand
{
    private readonly RentalService _service;
    private readonly Func<Task> _saveAsync;
    public string Key => "3";
    public string Description => "Орендувати авто";

    public RentCarCommand(RentalService service, Func<Task> saveAsync)
    {
        _service = service;
        _saveAsync = saveAsync;
    }

    public async Task ExecuteAsync()
    {
        var availCars = _service.GetAvailableCars();
        var allClients = _service.GetAllClients();
        if (!availCars.Any()) { System.Console.WriteLine("Немає доступних авто."); return; }
        if (!allClients.Any()) { System.Console.WriteLine("Немає клієнтів."); return; }

        System.Console.WriteLine("\nДоступні авто:");
        for (int i = 0; i < availCars.Count; i++)
            System.Console.WriteLine($"  {i + 1}. {availCars[i]}");
        System.Console.Write("Оберіть номер авто: ");
        if (!int.TryParse(System.Console.ReadLine(), out int carIdx) || carIdx < 1 || carIdx > availCars.Count)
        { System.Console.WriteLine("Невірний вибір."); return; }

        System.Console.WriteLine("\nКлієнти:");
        for (int i = 0; i < allClients.Count; i++)
            System.Console.WriteLine($"  {i + 1}. {allClients[i]}");
        System.Console.Write("Оберіть номер клієнта: ");
        if (!int.TryParse(System.Console.ReadLine(), out int clientIdx) || clientIdx < 1 || clientIdx > allClients.Count)
        { System.Console.WriteLine("Невірний вибір."); return; }

        System.Console.Write("Дата початку (dd.MM.yyyy): ");
        if (!DateTime.TryParseExact(System.Console.ReadLine(), "dd.MM.yyyy", null,
            System.Globalization.DateTimeStyles.None, out DateTime start))
        { System.Console.WriteLine("Невірна дата."); return; }

        System.Console.Write("Дата кінця (dd.MM.yyyy): ");
        if (!DateTime.TryParseExact(System.Console.ReadLine(), "dd.MM.yyyy", null,
            System.Globalization.DateTimeStyles.None, out DateTime end))
        { System.Console.WriteLine("Невірна дата."); return; }

        var strategies = _service.GetAvailableStrategies();
        System.Console.WriteLine("\nТарифи:");
        for (int i = 0; i < strategies.Count; i++)
            System.Console.WriteLine($"  {i + 1}. {strategies[i]}");
        System.Console.Write("Оберіть тариф (Enter = стандартний): ");
        var stratInput = System.Console.ReadLine();
        var stratName = "Стандартний";
        if (int.TryParse(stratInput, out int stratIdx) && stratIdx >= 1 && stratIdx <= strategies.Count)
            stratName = strategies[stratIdx - 1];

        var rental = _service.RentCar(allClients[clientIdx - 1].Id, availCars[carIdx - 1].Id, start, end, stratName);
        await _saveAsync();
        System.Console.WriteLine($"\n✓ Оренду оформлено!\n  {rental}");
    }
}