namespace CarRental.Domain;

public class Rental
{
    public Guid Id { get; private set; }
    public Car Car { get; private set; }
    public Client Client { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public RentalStatus Status { get; private set; }
    public decimal TotalCost { get; private set; }

    public Rental(Car car, Client client, DateTime startDate, DateTime endDate)
    {
        if (car == null) throw new ArgumentNullException(nameof(car));
        if (client == null) throw new ArgumentNullException(nameof(client));
        if (endDate <= startDate) throw new ArgumentException("End date must be after start date");
        if (!car.IsAvailable) throw new InvalidOperationException("Car is not available");

        Id = Guid.NewGuid();
        Car = car;
        Client = client;
        StartDate = startDate;
        EndDate = endDate;
        Status = RentalStatus.Active;
        TotalCost = CalculateCost();
    }

    public decimal CalculateCost()
    {
        int days = (int)(EndDate - StartDate).TotalDays;
        return days * Car.PricePerDay;
    }

    public void Complete()
    {
        if (Status != RentalStatus.Active) throw new InvalidOperationException("Only active rentals can be completed");
        Status = RentalStatus.Completed;
    }

    public void Cancel()
    {
        if (Status != RentalStatus.Active) throw new InvalidOperationException("Only active rentals can be cancelled");
        Status = RentalStatus.Cancelled;
    }

    public override string ToString() =>
        $"Оренда #{Id.ToString()[..8]} | {Car} | {Client.FullName} | {StartDate:dd.MM.yyyy}–{EndDate:dd.MM.yyyy} | {TotalCost} грн | {Status}";
}