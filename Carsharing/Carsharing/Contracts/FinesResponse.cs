namespace Carsharing.Contracts;

public record FinesResponse(
    int Id, 
    int TripId, 
    int StatusId, 
    string? Type, 
    decimal Amount,
    DateTime Date);