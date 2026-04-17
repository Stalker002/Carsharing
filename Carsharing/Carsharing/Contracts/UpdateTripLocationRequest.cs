namespace Carsharing.Contracts;

public record UpdateTripLocationRequest(
    string Location,
    double CarLatitude,
    double CarLongitude
);
