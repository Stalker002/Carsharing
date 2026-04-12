namespace Shared.Contracts.Trip;

public record CurrentTripDto(
    int Id,
    DateTime StartTime,
    string? TariffType,
    int CarId,
    string? CarBrand,
    string? CarModel,
    string? CarImage,
    string? CarLocation,
    double? CarLatitude,
    double? CarLongitude,
    decimal PricePerMinute,
    decimal PricePerKm,
    decimal PricePerDay
);

public record FinishTripRequest(
    int TripId,
    decimal Distance,
    string EndLocation,
    double CarLatitude,
    double CarLongitude,
    decimal FuelLevel
);

public record TripFinishResult(
    int BillId,
    decimal? TotalAmount,
    string Message
);
