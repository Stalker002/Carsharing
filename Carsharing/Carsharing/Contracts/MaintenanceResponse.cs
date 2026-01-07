namespace Carsharing.Contracts;

public record MaintenanceResponse(
    int Id, 
    int CarId, 
    string? WorkType, 
    string? Description, 
    decimal Cost, 
    DateOnly Date);