namespace Shared.Contracts.Cars;

public record CarWithMinInfoDto(
    int Id,
    string StatusName,
    decimal PricePerDay,
    decimal PricePerMinute,
    decimal PricePerKm,
    string? CategoryName,
    string? FuelType,
    decimal MaxFuel,
    decimal FuelLevel,
    string? Brand,
    string? Model,
    string? Transmission,
    double? Latitude,
    double? Longitude,
    string? StateNumber,
    string? ImagePath);