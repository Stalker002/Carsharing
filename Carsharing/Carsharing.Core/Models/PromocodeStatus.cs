namespace Carsharing.Core.Models;

public class PromocodeStatus
{
    private const int MaxStatusName = 64;

    private PromocodeStatus(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }
    public string Name { get; }

    public static (PromocodeStatus promocodeStatus, List<string>? erors) Create(int id, string name)
    {
        var errors = new List<string>();

        var nameError = string.Empty;

        if (name is { Length: > MaxStatusName })
            nameError = $"Status name can't be longer than {MaxStatusName} symbols";
        if (!string.IsNullOrEmpty(nameError)) errors.Add(nameError);

        if (errors.Count > 0)
            return (null, errors)!;

        var promocodeStatus = new PromocodeStatus(id, name);

        return (promocodeStatus, []);
    }
}