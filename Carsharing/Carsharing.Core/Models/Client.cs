using System.Text.RegularExpressions;

namespace Carsharing.Core.Models;

public class Client
{
    public const int MaxNameLength = 128;
    public const int MaxSurnameLength = 128;
    public const int MaxPhoneLength = 32;
    public const int MaxEmailLength = 128;

    private Client(int id, int userId, string? name, string? surname, string? phoneNumber, string? email)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Surname = surname;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public int Id { get; }

    public int UserId { get; set; }

    public string? Name { get; }

    public string? Surname { get; }

    public string? PhoneNumber { get; } = string.Empty;

    public string? Email { get; } = string.Empty;

    public static (Client client, string error) Create(int id, int userId, string? name, string? surname,
        string? phoneNumber, string? email)
    {
        var error = string.Empty;

        if (userId < 0)
            error = "User Id must be positive";

        if (string.IsNullOrWhiteSpace(name))
            error = "Name can't be empty";
        if (name is { Length: > MaxNameLength })
            error = $"Name can't be longer than {MaxNameLength} symbols";

        if (string.IsNullOrWhiteSpace(surname))
            error = "Surname can't be empty";
        if (surname is { Length: > MaxSurnameLength })
            error = $"Surname can't be longer than {MaxSurnameLength} symbols";

        if (string.IsNullOrWhiteSpace(phoneNumber))
            error = "Phone number can't be empty";
        if (phoneNumber is { Length: > MaxPhoneLength })
            error = $"Phone number can't be longer than {MaxPhoneLength} symbols";
        if (phoneNumber != null && !Regex.IsMatch(phoneNumber, @"^(\+375|80)(29|44|33|25)\d{7}$"))
            error = "Phone number should be in format +375XXXXXXXXX or 80XXXXXXXXX";

        if (string.IsNullOrWhiteSpace(email))
            error = "Email can't be empty";
        if (email is { Length: > MaxEmailLength })
            error = $"Email can't be longer than {MaxEmailLength} symbols";
        if (email != null && !Regex.IsMatch(email, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
            error = "Invalid email format";


        var client = new Client(id, userId, name?.Trim(), surname?.Trim(), phoneNumber?.Trim(), email?.Trim());

        return (client, error);
    }
}