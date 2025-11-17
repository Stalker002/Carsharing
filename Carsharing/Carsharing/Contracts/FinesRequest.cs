namespace Carsharing.Contracts;

public record FinesRequest(
    int TripId,
    int StatusId,
    string Type,
    decimal Amount,
    DateOnly Date);