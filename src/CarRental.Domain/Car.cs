namespace CarRental.Domain;

public class Car
{
    public Guid Id { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public decimal PricePerDay { get; private set; }
    public bool IsAvailable { get; private set; }

    public Car(string brand, string model, int year, decimal pricePerDay, Guid? id = null)
    {
        RentalValidator.ValidateCar(brand, model, year, pricePerDay);

        Id = id ?? Guid.NewGuid();
        Brand = brand;
        Model = model;
        Year = year;
        PricePerDay = pricePerDay;
        IsAvailable = true;
    }

    public void MakeAvailable() => IsAvailable = true;
    public void MakeUnavailable() => IsAvailable = false;

    public override string ToString() => $"{Brand} {Model} ({Year}) — {PricePerDay} грн/день";
}