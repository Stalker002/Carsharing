namespace Carsharing.Application.DTOs;

public record TripWithMinInfoDto(
    int Id,
    int BookingId,
    string StatusName,
    string TariffType,
    DateTime StartTime,
    DateTime? EndTime,
    decimal? Duration,
    decimal? Distance);