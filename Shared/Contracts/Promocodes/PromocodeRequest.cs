namespace Shared.Contracts.Promocodes;

public record PromocodeRequest(
    int StatusId,
    string Code,
    decimal Discount,
    DateOnly StartDate,
    DateOnly EndDate);