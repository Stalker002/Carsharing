using Microsoft.AspNetCore.Http;

namespace Carsharing.Application.DTOs;

public record CarUpdateDto(
    int StatusId,
    int CategoryId,
    string Location,
    decimal FuelLevel,
    string Brand,
    string Model,
    int Year,
    string Transmission,
    string FuelType,
    string VinNumber,
    string StateNumber,
    int Mileage,
    decimal MaxFuel,
    decimal FuelPerKm,
    string TariffName,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay,
    IFormFile? Image
);