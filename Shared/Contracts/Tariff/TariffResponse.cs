namespace Shared.Contracts.Tariff;

public record TariffResponse(
    int Id,
    string? Name,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay);