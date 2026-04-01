using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface ICategoriesService
{
    Task<List<Category>> GetCategories(CancellationToken cancellationToken);

    Task<int> CreateCategory(Category category, CancellationToken cancellationToken);

    Task<int> UpdateCategory(int id, string? name, CancellationToken cancellationToken);

    Task<int> DeleteCategory(int id, CancellationToken cancellationToken);
}