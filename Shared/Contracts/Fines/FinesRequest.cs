namespace Shared.Contracts.Fines;

public record FinesRequest(
    int TripId,
    int StatusId,
    string Type,
    decimal Amount,
    DateTime Date);