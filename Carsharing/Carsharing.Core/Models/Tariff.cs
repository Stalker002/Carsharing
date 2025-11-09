namespace Carsharing.Core.Models;

public class Tariff
{
    private const int MaxNameLength = 100;
    private Tariff(int id, string name, decimal pricePerMinute, decimal pricePerKm, 
        decimal pricePerDay)
    {
        Id = id;
        Name = name;
        PricePerMinute = pricePerMinute;
        PricePerKm = pricePerKm;
        PricePerDay = pricePerDay;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public static decimal PricePerMinute { get; set; }

    public static decimal PricePerKm { get; set; }

    public static decimal PricePerDay { get; set; }

    public static (Tariff tariff, string error) Create(int id, string name, decimal pricePerMinute, decimal pricePerKm,
        decimal pricePerDay)
    {
        var error = string.Empty;

        if (string.IsNullOrWhiteSpace(name))
            error = "Name can't be empty";
        if (name.Length > MaxNameLength)
            error = $"Type can't be longer than {MaxNameLength} symbols";

        if (PricePerMinute < 0)
            error = "Price per minute must be positive";

        if (PricePerKm < 0)
            error = "Price per km must be positive";

        if (PricePerDay < 0)
            error = "Price per day must be positive";

        var tariff = new Tariff(id, name, pricePerMinute, pricePerKm, pricePerDay);
        return (tariff, error);
    }
}