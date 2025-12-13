namespace Carsharing.Application.DTOs;

public record CarWithInfoAdminDto(
    int Id,
    string StatusName,
    string CategoryName,
    string Transmission,
    string Brand,
    string Model,
    int Year,
    string Location,
    string VinNumber,
    string StateNumber,
    string FuelType,
    decimal FuelLevel,
    decimal MaxFuel,
    decimal FuelPerKm,
    decimal Mileage,
    string TariffName,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay,
    string? Image
   );