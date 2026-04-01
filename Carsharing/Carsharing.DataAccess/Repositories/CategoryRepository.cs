using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CarsharingDbContext _context;

    public CategoryRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> Get()
    {
        var categoryEntities = await _context.Category
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToListAsync();

        var categories = categoryEntities
            .Select(c => Category.Create(
                c.Id,
                c.Name).category)
            .ToList();

        return categories;
    }

    public async Task<List<Category>> GetById(int id)
    {
        var categoryEntities = await _context.Category
            .Where(c => c.Id == id)
            .AsNoTracking()
            .ToListAsync();

        var categories = categoryEntities
            .Select(c => Category.Create(
                c.Id,
                c.Name).category)
            .ToList();

        return categories;
    }

    public async Task<int> Create(Category category)
    {
        var (_, error) = Category.Create(
            category.Id,
            category.Name);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Category create error: {error}");

        var categoryEntity = new CategoryEntity
        {
            Id = category.Id,
            Name = category.Name
        };

        await _context.Category.AddAsync(categoryEntity);
        await _context.SaveChangesAsync();

        return categoryEntity.Id;
    }

    public async Task<int> Update(int id, string? name)
    {
        var category = await _context.Category.FirstOrDefaultAsync(c => c.Id == id)
                       ?? throw new Exception("Category not found");

        if (!string.IsNullOrWhiteSpace(name))
            category.Name = name;

        var (_, error) = Category.Create(
            category.Id,
            category.Name);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Category create error: {error}");

        await _context.SaveChangesAsync();

        return category.Id;
    }

    public async Task<int> Delete(int id)
    {
        var categoryEntity = await _context.Category
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}