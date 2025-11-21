namespace Carsharing.Application.DTOs;

public record ReviewWithClientInfo(
    int Id,
    string Name,
    string Surname,
    short Rating,
    string Comment,
    DateTime Date);