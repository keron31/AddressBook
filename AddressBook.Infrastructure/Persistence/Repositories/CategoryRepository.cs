using AddressBook.Application.Common.Interfaces.Persistence;
using AddressBook.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AddressBookDbContext _dbContext;
    private readonly List<string> DefaultCategoryNames = new List<string>
    {
        "Służbowy",
        "Prywatny",
        "Inny"
    };
    private readonly List<string> DefaultSubCategoryNames_1 = new List<string>
    {
        "Szef",
        "Kierownik",
        "Pracownik",
        "Klient",
        "Dostawca",
        "Partner",
        "Inny"
    };

    public CategoryRepository(AddressBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddDefaultCategoryAsync()
    {
        // if there are no default categories then add them
        if (!_dbContext.Categories.Any())
        {
            var defaultCategories = new List<Category>();
            foreach (var categoryName in DefaultCategoryNames)
            {
                var category = new Category
                {
                    Name = categoryName
                };
                if (categoryName == "Służbowy")
                {
                    var subCategories = new List<SubCategory>();
                    foreach (var subCategoryName in DefaultSubCategoryNames_1)
                    {
                        subCategories.Add(new SubCategory
                        {
                            Name = subCategoryName
                        });
                    }
                    category.SubCategories = subCategories;
                }
                defaultCategories.Add(category);
            }
            await _dbContext.Categories.AddRangeAsync(defaultCategories);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task<List<SubCategory>> GetSubCategoriesAsync()
    {
        return await _dbContext.SubCategories.ToListAsync();
    }
}