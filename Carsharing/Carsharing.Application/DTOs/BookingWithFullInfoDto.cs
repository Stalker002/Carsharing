namespace Carsharing.Application.DTOs;

public record BookingWithFullInfoDto(
    int Id,
    string Status,
    string ClientFullName,
    string CarName,
    DateTime? StartDate,
    DateTime? EndDate);