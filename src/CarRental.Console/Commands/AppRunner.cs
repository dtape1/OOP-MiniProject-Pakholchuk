namespace CarRental.Console;

public class AppRunner
{
    private readonly Dictionary<string, Commands.ICommand> _commands;
    private readonly Func<Task> _saveAsync;

    public AppRunner(IEnumerable<Commands.ICommand> commands, Func<Task> saveAsync)
    {
        _commands = commands.ToDictionary(c => c.Key);
        _saveAsync = saveAsync;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            PrintMenu();
            var input = System.Console.ReadLine();
            if (input == "0") { await _saveAsync(); System.Console.WriteLine("До побачення!"); return; }
            if (_commands.TryGetValue(input ?? "", out var command))
            {
                try { await command.ExecuteAsync(); }
                catch (Exception ex) { System.Console.WriteLine($"\n⚠ Помилка: {ex.Message}"); }
            }
            else { System.Console.WriteLine("Невідома опція."); }
        }
    }

    private void PrintMenu()
    {
        System.Console.WriteLine("\n=== Система оренди автомобілів ===");
        foreach (var cmd in _commands.Values)
            System.Console.WriteLine($"{cmd.Key}. {cmd.Description}");
        System.Console.WriteLine("0. Вихід");
        System.Console.Write("Оберіть опцію: ");
    }
}