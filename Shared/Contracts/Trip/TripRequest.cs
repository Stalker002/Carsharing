namespace Shared.Contracts.Trip;

public record TripRequest(
    int BookingId,
    int StatusId,
    string TariffType,
    DateTime StartTime,
    DateTime? EndTime,
    decimal? Duration,
    decimal? Distance);