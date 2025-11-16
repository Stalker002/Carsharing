using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class CategoriesService : ICategoriesService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> GetCategories()
    {
        return await _categoryRepository.Get();
    }

    public async Task<int> CreateCategory(Category category)
    {
        return await _categoryRepository.Create(category);
    }

    public async Task<int> UpdateCategory(int id, string? name)
    {
        return await _categoryRepository.Update(id, name);
    }

    public async Task<int> DeleteCategory(int id)
    {
        return await _categoryRepository.Delete(id);
    }
}