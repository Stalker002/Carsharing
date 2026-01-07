namespace Carsharing.Contracts;

public record CarsResponse(
    int Id,
    int StatusId,
    int TariffId,
    int CategoryId,
    int SpecificationId,
    string? Location,
    decimal FuelLevel,
    string? ImagePath);