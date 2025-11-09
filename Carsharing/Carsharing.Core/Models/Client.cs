using System.Text.RegularExpressions;

namespace Carsharing.Core.Models;

public class Client
{
    private const int MaxNameLength = 128;
    private const int MaxSurnameLength = 128;
    private const int MaxPhoneLength = 32;
    private const int MaxEmailLength = 128;

    private Client(int id, int userId, string name, string surname, string phoneNumber, string email)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Surname = surname;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public static (Client client, string error) Create(int id, int userId, string name, string surname,
        string phoneNumber, string email)
    {
        var error = string.Empty;

        if (userId < 0)
            error = "User Id must be positive";

        if (string.IsNullOrWhiteSpace(name))
            error = "Name can't be empty";
        if (name.Length > MaxNameLength)
            error = $"Name can't be longer than {MaxNameLength} symbols";

        if (string.IsNullOrWhiteSpace(surname))
            error = "Surname can't be empty";
        if (surname.Length > MaxSurnameLength)
            error = $"Surname can't be longer than {MaxSurnameLength} symbols";

        if (string.IsNullOrWhiteSpace(phoneNumber))
            error = "Phone number can't be empty";
        if (phoneNumber.Length > MaxPhoneLength)
            error = $"Phone number can't be longer than {MaxPhoneLength} symbols";
        if (!Regex.IsMatch(phoneNumber, @"^(375|80)(29|44|33|25)\d{7}$"))
            error = "Phone number should be in format +375XXXXXXXXX or 80XXXXXXXXX";

        if (string.IsNullOrWhiteSpace(email))
            error = "Email can't be empty";
        if (email.Length > MaxEmailLength)
            error = $"Email can't be longer than {MaxEmailLength} symbols";
        if (!Regex.IsMatch(email, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
            error = "Invalid email format";


        var client = new Client(id, userId, name.Trim(), surname.Trim(), phoneNumber.Trim(), email.Trim());

        return (client, error);
    }
}