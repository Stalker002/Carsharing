namespace Carsharing.Core.Models;

public class InsuranceStatus
{
    private const int MaxStatusName = 64;

    private InsuranceStatus(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }
    public string Name { get; }

    public static (InsuranceStatus insuranceStatus, List<string>? erors) Create(int id, string name)
    {
        var errors = new List<string>();

        var nameError = string.Empty;

        if (name is { Length: > MaxStatusName })
            nameError = $"Status name can't be longer than {MaxStatusName} symbols";
        if (!string.IsNullOrEmpty(nameError)) errors.Add(nameError);

        if (errors.Count > 0)
            return (null, errors)!;

        var insuranceStatus = new InsuranceStatus(id, name);

        return (insuranceStatus, []);
    }
}