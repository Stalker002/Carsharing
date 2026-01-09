namespace Carsharing.Application.DTOs;

public record ReviewCreateRequest(
    int ClientId,
    int TripId,
    short Rating,
    string Comment,
    DateTime Date);