using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ICategoryRepository
{
    Task<List<Category>> Get(CancellationToken cancellationToken);

    Task<List<Category>> GetById(int id, CancellationToken cancellationToken);

    Task<int> Create(Category category, CancellationToken cancellationToken);

    Task<int> Update(int id, string? name, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}