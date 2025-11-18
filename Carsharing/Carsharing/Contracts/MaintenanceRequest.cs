namespace Carsharing.Contracts;

public record MaintenanceRequest(
    int CarId,
    string WorkType,
    string Description,
    decimal Cost,
    DateOnly Date);