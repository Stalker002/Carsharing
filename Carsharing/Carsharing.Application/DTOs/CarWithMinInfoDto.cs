namespace Carsharing.Application.DTOs;

public record CarWithMinInfoDto(
    int Id,
    string StatusName,
    decimal PricePerDay,
    string CategoryName,
    string FuelType,
    string Brand,
    string Model,
    string Transmission,
    string? ImagePath);