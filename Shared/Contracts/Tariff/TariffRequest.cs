namespace Shared.Contracts.Tariff;

public record TariffRequest(
    string Name,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay);