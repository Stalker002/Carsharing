namespace Carsharing.Core.Models;

public class BillStatus
{
    private const int MaxStatusName = 64;
    public BillStatus(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }
    public string Name { get; }

    public static (BillStatus billStatus, List<string>? erors) Create(int id, string name)
    {
        var errors = new List<string>();

        var nameError = string.Empty;

        if (name is { Length: > MaxStatusName })
            nameError = $"Status name can't be longer than {MaxStatusName} symbols";
        if (!string.IsNullOrEmpty(nameError)) errors.Add(nameError);

        if (errors.Any())
            return (null, errors);

        var billStatus = new BillStatus(id, name);

        return (billStatus, new List<string>());
    }
}