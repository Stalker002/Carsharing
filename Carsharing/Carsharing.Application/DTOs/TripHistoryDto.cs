namespace Carsharing.Application.DTOs;

public record TripHistoryDto
(
    int Id,
    string? CarBrand,
    int CarId,
    string? CarModel, 
    string? CarImage, 
    string StatusName, 
    DateTime StartTime, 
    DateTime? EndTime, 
    decimal TotalAmount,
    string? TariffType,
    decimal? Duration, 
    decimal? Distance,
    bool InsuranceActive,
    decimal? FuelUsed,
    decimal? Refueled,
    string? StartLocation, 
    string? EndLocation
);