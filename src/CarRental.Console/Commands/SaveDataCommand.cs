namespace CarRental.Console.Commands;

public class SaveDataCommand : ICommand
{
    private readonly Func<Task> _saveAsync;
    public string Key => "8";
    public string Description => "Зберегти дані";

    public SaveDataCommand(Func<Task> saveAsync) => _saveAsync = saveAsync;

    public async Task ExecuteAsync()
    {
        await _saveAsync();
        System.Console.WriteLine("✓ Дані збережено!");
    }
}