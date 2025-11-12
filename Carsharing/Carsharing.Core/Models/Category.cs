namespace Carsharing.Core.Models;

public class Category
{
    public const int MaxNameLength = 100;
    private Category(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }

    public string Name { get; }

    public static (Category category, string error) Create(int id, string name)
    {
        var error = string.Empty;

        if (string.IsNullOrWhiteSpace(name))
            error = "Name can't be empty";

        if (name.Length > MaxNameLength)
            error = $"Name can't be longer than {MaxNameLength} symbols";

        var category = new Category(id, name);
        return (category, error);
    }
}