namespace Shared.Contracts.Statuses;

public record StatusResponse(
    int Id,
    string Name,
    string? Description);