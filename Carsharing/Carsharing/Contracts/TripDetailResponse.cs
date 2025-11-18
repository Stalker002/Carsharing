namespace Carsharing.Contracts;

public record TripDetailResponse(
    int Id, 
    int TripId, 
    string StartLocation, 
    string EndLocation, 
    bool InsuranceActive,
    decimal? FuelUsed, 
    decimal? Refueled);