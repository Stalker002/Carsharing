using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ICategoriesService
{
    Task<List<Category>> GetCategories();
    Task<int> CreateCategory(Category category);
    Task<int> UpdateCategory(int id, string? name);
    Task<int> DeleteCategory(int id);
}