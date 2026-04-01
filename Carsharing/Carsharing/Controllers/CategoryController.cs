using Carsharing.Application.Abstractions;
using Carsharing.Contracts;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoriesService _categoriesService;

    public CategoryController(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    [HttpGet]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<CategoriesResponse>>> GetCategories()
    {
        var categories = await _categoriesService.GetCategories();
        var response = categories.Select(c => new CategoriesResponse(c.Id, c.Name));

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> CreateCategory([FromBody] CategoriesRequest request)
    {
        var (category, error) = Category.Create(
            0,
            request.Name);

        if (!string.IsNullOrEmpty(error)) return BadRequest(error);

        var categoryId = await _categoriesService.CreateCategory(category);

        return Ok(categoryId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateCategory(int id, [FromBody] CategoriesRequest request)
    {
        var categoryId = await _categoriesService.UpdateCategory(id, request.Name);

        return Ok(categoryId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteCategory(int id)
    {
        return Ok(await _categoriesService.DeleteCategory(id));
    }
}