namespace Carsharing.Contracts;

public record PromocodeRequest(
    int StatusId,
    string Code,
    decimal Discount,
    DateOnly StartDate,
    DateOnly EndDate);