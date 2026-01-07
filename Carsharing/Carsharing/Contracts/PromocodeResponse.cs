namespace Carsharing.Contracts;

public record PromocodeResponse(
    int Id, 
    int StatusId, 
    string? Code, 
    decimal Discount, 
    DateOnly StartDate, 
    DateOnly EndDate);