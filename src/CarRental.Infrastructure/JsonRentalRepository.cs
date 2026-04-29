using System.Text.Json;
using CarRental.Domain;
using CarRental.Domain.Interfaces;

namespace CarRental.Infrastructure;

public class JsonRentalRepository : IRentalRepository
{
    private readonly string _filePath;
    private readonly ICarRepository _carRepo;
    private readonly IClientRepository _clientRepo;
    private List<Rental> _rentals = new();

    public JsonRentalRepository(string filePath, ICarRepository carRepo, IClientRepository clientRepo)
    {
        _filePath = filePath;
        _carRepo = carRepo;
        _clientRepo = clientRepo;
    }

    public void Add(Rental rental) => _rentals.Add(rental);

    public Rental? GetById(Guid id) => _rentals.FirstOrDefault(r => r.Id == id);

    public List<Rental> GetAll() => _rentals.ToList();

    public List<Rental> GetActive() => _rentals.Where(r => r.Status == RentalStatus.Active).ToList();

    public async Task SaveAsync()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        var dtos = _rentals.Select(r => new RentalDto
        {
            Id = r.Id,
            CarId = r.Car.Id,
            ClientId = r.Client.Id,
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            Status = r.Status.ToString(),
            TotalCost = r.TotalCost,
            PricingStrategyName = r.PricingStrategyName
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
            var dtos = JsonSerializer.Deserialize<List<RentalDto>>(json) ?? new();
            _rentals = dtos.Select(d =>
            {
                var car = _carRepo.GetById(d.CarId)!;
                var client = _clientRepo.GetById(d.ClientId)!;
                var rental = new Rental(car, client, d.StartDate, d.EndDate, d.PricingStrategyName, d.Id);
                rental.SetTotalCost(d.TotalCost);
                if (d.Status == "Completed") rental.Complete();
                else if (d.Status == "Cancelled") rental.Cancel();
                return rental;
            }).ToList();
        }
        catch (JsonException)
        {
            Console.WriteLine("⚠ Пошкоджений файл rentals.json — починаємо з порожнього списку");
            _rentals = new();
        }
    }

    private class RentalDto
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "";
        public decimal TotalCost { get; set; }
        public string PricingStrategyName { get; set; } = "";
    }
}