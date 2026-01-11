using Microsoft.AspNetCore.Http;

namespace Carsharing.Application.DTOs;

public record CarsCreateRequest(
    int StatusId,
    string TariffName,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay,
    int CategoryId,
    string FuelType,
    string Brand,
    string Model,
    string Transmission,
    int Year,
    string VinNumber,
    string StateNumber,
    int Mileage,
    decimal MaxFuel,
    decimal FuelPerKm,
    string Location,
    decimal FuelLevel,
    IFormFile Image);