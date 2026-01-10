namespace Carsharing.Application.DTOs;

public record CurrentTripDto(
    int Id,
    DateTime StartTime,
    string? TariffType,
    int CarId,
    string? CarBrand,
    string? CarModel,
    string? CarImage,
    string? CarLocation,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay
);

public record FinishTripRequest(
    int TripId,
    decimal Distance,
    string EndLocation,
    decimal FuelLevel
);

public record TripFinishResult(
    int BillId,
    decimal? TotalAmount,
    string Message
);