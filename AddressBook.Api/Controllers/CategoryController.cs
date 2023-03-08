using AddressBook.Application.Common.Interfaces.Persistence;
using AddressBook.Application.Services.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Api.Controllers;

[ApiController]
[Authorize]
[Route("categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRepository.GetCategoriesAsync();
        var subCategories = await _categoryRepository.GetSubCategoriesAsync();

        var response = new List<CategoryResponse>();
        foreach (var category in categories)
        {
            var subCategoryList = new List<string>();
            foreach (var subCategory in subCategories)
            {
                if (subCategory.CategoryId == category.Id)
                {
                    subCategoryList.Add(subCategory.Name);
                }
            }

            response.Add(new CategoryResponse(category.Id, category.Name, subCategoryList));
        }

        return Ok(response);
    }
}