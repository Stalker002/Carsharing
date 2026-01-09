namespace Carsharing.Core.Models;

public class TripStatus
{
    private const int MaxStatusName = 64;

    private TripStatus(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }
    public string Name { get; }

    public static (TripStatus tripStatus, List<string>? erors) Create(int id, string name)
    {
        var errors = new List<string>();

        var nameError = string.Empty;

        if (name is { Length: > MaxStatusName })
            nameError = $"Status name can't be longer than {MaxStatusName} symbols";
        if (!string.IsNullOrEmpty(nameError)) errors.Add(nameError);

        if (errors.Any())
            return (null, errors);

        var tripStatus = new TripStatus(id, name);

        return (tripStatus, new List<string>());
    }
}