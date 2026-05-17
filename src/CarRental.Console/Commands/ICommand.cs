namespace CarRental.Console.Commands;

public interface ICommand
{
    string Key { get; }
    string Description { get; }
    Task ExecuteAsync();
}