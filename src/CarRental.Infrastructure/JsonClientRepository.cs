using System.Text.Json;
using CarRental.Domain;
using CarRental.Domain.Interfaces;

namespace CarRental.Infrastructure;

public class JsonClientRepository : IClientRepository
{
    private readonly string _filePath;
    private List<Client> _clients = new();

    public JsonClientRepository(string filePath = "data/clients.json")
    {
        _filePath = filePath;
    }

    public void Add(Client client) => _clients.Add(client);

    public Client? GetById(Guid id) => _clients.FirstOrDefault(c => c.Id == id);

    public List<Client> GetAll() => _clients.ToList();

    public async Task SaveAsync()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        var dtos = _clients.Select(c => new ClientDto
        {
            Id = c.Id,
            FullName = c.FullName,
            Email = c.Email,
            Phone = c.Phone
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
            var dtos = JsonSerializer.Deserialize<List<ClientDto>>(json) ?? new();
            _clients = dtos.Select(d => new Client(d.FullName, d.Email, d.Phone, d.Id)).ToList();
        }
        catch (JsonException)
        {
            Console.WriteLine("⚠ Пошкоджений файл clients.json — починаємо з порожнього списку");
            _clients = new();
        }
    }

    private class ClientDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }
}