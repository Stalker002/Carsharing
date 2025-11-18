namespace Carsharing.Contracts;

public record TariffRequest(
    string Name,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay);