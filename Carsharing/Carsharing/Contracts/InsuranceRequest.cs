namespace Carsharing.Contracts;

public record InsuranceRequest(
    int CarId,
    int StatusId,
    string Type,
    string Company,
    string PolicyNumber,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal Cost);