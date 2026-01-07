namespace Carsharing.Core.Models;

public class Tariff
{
    public const int MaxNameLength = 100;

    private Tariff(int id, string? name, decimal pricePerMinute, decimal pricePerKm,
        decimal pricePerDay)
    {
        Id = id;
        Name = name;
        PricePerMinute = pricePerMinute;
        PricePerKm = pricePerKm;
        PricePerDay = pricePerDay;
    }

    public int Id { get; }

    public string? Name { get; }

    public decimal PricePerMinute { get; }

    public decimal PricePerKm { get; }

    public decimal PricePerDay { get; }

    public static (Tariff tariff, string error) Create(int id, string? name, decimal pricePerMinute, decimal pricePerKm,
        decimal pricePerDay)
    {
        var error = string.Empty;

        if (string.IsNullOrWhiteSpace(name))
            error = "Name can't be empty";
        if (name is { Length: > MaxNameLength })
            error = $"Type can't be longer than {MaxNameLength} symbols";

        if (pricePerMinute <= 0)
            error = "Price per minute must be positive";

        if (pricePerKm <= 0)
            error = "Price per km must be positive";

        if (pricePerDay <= 0)
            error = "Price per day must be positive";

        var tariff = new Tariff(id, name, pricePerMinute, pricePerKm, pricePerDay);
        return (tariff, error);
    }
}