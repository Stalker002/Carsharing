namespace Carsharing.Contracts;

public record CarsRequest(
    int StatusId, 
    int TariffId, 
    int CategoryId, 
    int SpecificationId,
    string Location, 
    decimal FuelLevel);