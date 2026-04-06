namespace Shared.Contracts.Insurances;

public record InsurancesResponse(
    int Id,
    int CarId,
    int StatusId,
    string? Type,
    string? Company,
    string? PolicyNumber,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal Cost);