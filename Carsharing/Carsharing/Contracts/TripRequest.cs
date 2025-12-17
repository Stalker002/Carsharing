namespace Carsharing.Contracts;

public record TripRequest(
    int BookingId,
    int StatusId,
    int? CarId,
    string TariffType,
    DateTime StartTime,
    DateTime? EndTime,
    decimal? Duration,
    decimal? Distance);