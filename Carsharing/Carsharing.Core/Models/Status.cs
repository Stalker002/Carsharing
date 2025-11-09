namespace Carsharing.Core.Models;

public class Status
{
    private const int MaxNameLength = 50;
    private Status (int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public static (Status status, string error) Create(int id, string name, string description)
    {
        var error = string.Empty;

        if (string.IsNullOrWhiteSpace(name))
            error = "Name can't be empty";
        if (name.Length > MaxNameLength)
            error = $"Status name can't be longer than {MaxNameLength} symbols";

        var status = new Status (id, name, description);
        return (status, error);
    }
}