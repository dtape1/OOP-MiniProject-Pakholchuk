using System.Text.Json;
using CarRental.Domain;
using CarRental.Domain.Interfaces;

namespace CarRental.Infrastructure;

public class JsonCarRepository : ICarRepository
{
    private readonly string _filePath;
    private List<Car> _cars = new();

    public JsonCarRepository(string filePath = "data/cars.json")
    {
        _filePath = filePath;
    }

    public void Add(Car car) => _cars.Add(car);

    public Car? GetById(Guid id) => _cars.FirstOrDefault(c => c.Id == id);

    public List<Car> GetAll() => _cars.ToList();

    public List<Car> GetAvailable() => _cars.Where(c => c.IsAvailable).ToList();

    public async Task SaveAsync()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        var dtos = _cars.Select(c => new CarDto
        {
            Id = c.Id,
            Brand = c.Brand,
            Model = c.Model,
            Year = c.Year,
            PricePerDay = c.PricePerDay,
            IsAvailable = c.IsAvailable
        }).ToList();
        var json = JsonSerializer.Serialize(dtos, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task LoadAsync()
    {
        if (!File.Exists(_filePath)) return;
        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            var dtos = JsonSerializer.Deserialize<List<CarDto>>(json) ?? new();
            _cars = dtos.Select(d =>
            {
                var car = new Car(d.Brand, d.Model, d.Year, d.PricePerDay, d.Id);
                if (!d.IsAvailable) car.MakeUnavailable();
                return car;
            }).ToList();
        }
        catch (JsonException)
        {
            Console.WriteLine("⚠ Пошкоджений файл cars.json — починаємо з порожнього списку");
            _cars = new();
        }
    }

    private class CarDto
    {
        public Guid Id { get; set; }
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
    }
}