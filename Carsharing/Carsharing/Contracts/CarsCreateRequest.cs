namespace Carsharing.Contracts;

public record CarsCreateRequest(
    int StatusId,
    string Name,
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