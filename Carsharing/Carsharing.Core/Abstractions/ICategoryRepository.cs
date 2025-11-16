using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ICategoryRepository
{
    Task<List<Category>> Get();
    Task<int> Create(Category category);
    Task<int> Update(int id, string? name);
    Task<int> Delete(int id);
}