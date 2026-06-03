namespace CarRental.Domain;

public static class RentalValidator
{
    public static void ValidateCar(string brand, string model, int year, decimal pricePerDay)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Brand cannot be empty");
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model cannot be empty");
        if (year < 1990 || year > DateTime.Now.Year)
            throw new ArgumentException("Invalid year");
        if (pricePerDay <= 0)
            throw new ArgumentException("Price must be positive");
    }

    public static void ValidateClient(string fullName, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty");
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty");
    }

    public static void ValidateRental(Car car, Client client, DateTime startDate, DateTime endDate)
    {
        if (car == null) throw new ArgumentNullException(nameof(car));
        if (client == null) throw new ArgumentNullException(nameof(client));
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date");
        if (!car.IsAvailable)
            throw new InvalidOperationException("Car is not available");
    }
}