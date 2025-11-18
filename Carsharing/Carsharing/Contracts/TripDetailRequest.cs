namespace Carsharing.Contracts;

public record TripDetailRequest(
    int TripId,
    string StartLocation,
    string EndLocation,
    bool InsuranceActive,
    decimal? FuelUsed,
    decimal? Refueled);