namespace Shared.Contracts.Trip;

public record UpdateTripLocationRequest(
    string Location,
    double CarLatitude,
    double CarLongitude
);
