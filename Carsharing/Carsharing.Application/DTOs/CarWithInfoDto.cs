namespace Carsharing.Application.DTOs;

public record CarWithInfoDto(
    int Id,
    string StatusName,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay,
    string CategoryName,
    string FuelType,
    string Model,
    string Transmission,
    int Year,
    string StateNumber,
    decimal MaxFuel,
    string Location,
    decimal FuelLevel,
    string? ImagePath);