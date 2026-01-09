namespace Carsharing.Core.Models;

public class BookingStatus
{
    private const int MaxStatusName = 64;

    private BookingStatus(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }
    public string Name { get; }

    public static (BookingStatus bookingStatus, List<string>? erors) Create(int id, string name)
    {
        var errors = new List<string>();

        var nameError = string.Empty;

        if (name is { Length: > MaxStatusName })
            nameError = $"Status name can't be longer than {MaxStatusName} symbols";
        if (!string.IsNullOrEmpty(nameError)) errors.Add(nameError);

        if (errors.Any())
            return (null, errors);

        var bookingStatus = new BookingStatus(id, name);

        return (bookingStatus, new List<string>());
    }
}