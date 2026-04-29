namespace CarRental.Domain;

public class Client
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }

    public Client(string fullName, string email, string phone, Guid? id = null)
    {
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name cannot be empty");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty");
        if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Phone cannot be empty");

        Id = id ?? Guid.NewGuid();
        FullName = fullName;
        Email = email;
        Phone = phone;
    }

    public override string ToString() => $"{FullName} ({Email})";
}