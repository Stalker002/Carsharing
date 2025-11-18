namespace Carsharing.Contracts;

public record SpecificationCarRequest(
    string FuelType,
    string Brand,
    string Model,
    string Transmission,
    int Year,
    string VinNumber,
    string StateNumber,
    int Mileage,
    decimal MaxFuel,
    decimal FuelPerKm);