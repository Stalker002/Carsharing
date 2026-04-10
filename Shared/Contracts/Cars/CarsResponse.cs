namespace Shared.Contracts.Cars;

public record CarsResponse(
    int Id,
    int StatusId,
    int TariffId,
    int CategoryId,
    int SpecificationId,
    string? Location,
    double? Latitude,
    double? Longitude,
    decimal FuelLevel,
    string? ImagePath);
