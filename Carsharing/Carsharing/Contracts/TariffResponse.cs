namespace Carsharing.Contracts;

public record TariffResponse(
    int Id,
    string? Name,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay);