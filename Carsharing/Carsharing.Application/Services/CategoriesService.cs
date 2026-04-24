using Carsharing.Application.Abstractions;
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

    public async Task<List<Category>> GetCategories(CancellationToken cancellationToken)
    {
        return await _categoryRepository.Get(cancellationToken);
    }

    public async Task<int> CreateCategory(Category category, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Create(category, cancellationToken);
    }

    public async Task<int> UpdateCategory(int id, string? name, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Update(id, name, cancellationToken);
    }

    public async Task<int> DeleteCategory(int id, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Delete(id, cancellationToken);
    }
}