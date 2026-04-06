namespace Shared.Contracts.Cars;

public record CarWithMinInfoDto(
    int Id,
    string StatusName,
    decimal PricePerDay,
    string? CategoryName,
    string? FuelType,
    decimal MaxFuel,
    string? Brand,
    string? Model,
    string? Transmission,
    string? ImagePath);