namespace Carsharing.Contracts;

public record TripResponse(
    int Id, 
    int BookingId, 
    int StatusId, 
    string? TariffType, 
    DateTime StartTime,
    DateTime? EndTime, 
    decimal? Duration, 
    decimal? Distance);