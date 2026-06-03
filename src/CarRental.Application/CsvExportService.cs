using CarRental.Domain;

namespace CarRental.Application;

public class CsvExportService
{
    private readonly RentalService _service;

    public CsvExportService(RentalService service)
    {
        _service = service;
    }

    public async Task ExportRentalsAsync(string filePath)
    {
        var rentals = _service.FilterRentals(_ => true);
        var lines = new List<string>
        {
            "Id,Car,Client,StartDate,EndDate,Status,TotalCost"
        };

        foreach (var r in rentals)
        {
            lines.Add($"{r.Id.ToString()[..8]}," +
                      $"{r.Car.Brand} {r.Car.Model}," +
                      $"{r.Client.FullName}," +
                      $"{r.StartDate:dd.MM.yyyy}," +
                      $"{r.EndDate:dd.MM.yyyy}," +
                      $"{r.Status}," +
                      $"{r.TotalCost}");
        }

        await File.WriteAllLinesAsync(filePath, lines);
    }
}