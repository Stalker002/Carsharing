namespace Shared.Contracts.Cars;

public record CarWithInfoDto(
    int Id,
    string StatusName,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay,
    string? CategoryName,
    string? FuelType,
    string? Brand,
    string? Model,
    string? Transmission,
    int Year,
    string StateNumber,
    decimal MaxFuel,
    string? Location,
    double? Latitude,
    double? Longitude,
    decimal FuelLevel,
    string? ImagePath);
