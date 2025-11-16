using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IStatusRepository
{
    Task<List<Status>> Get();
    Task<int> Create(Status status);
    Task<int> Update(int id, string? name, string? description);
    Task<int> Delete(int id);
}