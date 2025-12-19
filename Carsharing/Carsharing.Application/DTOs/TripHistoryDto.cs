namespace Carsharing.Application.DTOs;

public record TripHistoryDto
(
    int Id,
    string? CarBrand,
    string? CarModel, 
    string? CarImage, 
    string StatusName, 
    DateTime StartTime, 
    DateTime? EndTime, 
    decimal TotalAmount, 
    string? TariffType,
    decimal? Duration, 
    decimal? Distance, 
    string StartLocation, 
    string EndLocation
);