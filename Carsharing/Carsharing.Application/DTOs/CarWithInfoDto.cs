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
    string VinNumber,
    string StateNumber,
    int Mileage,
    decimal MaxFuel,
    decimal FuelPerKm,
    string Location,
    decimal FuelLevel);