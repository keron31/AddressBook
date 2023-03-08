using AddressBook.Domain.Models;

namespace AddressBook.Application.Common.Interfaces.Persistence;

public interface ICategoryRepository
{
    Task AddDefaultCategoryAsync();
    Task<List<Category>> GetCategoriesAsync();
    Task<List<SubCategory>> GetSubCategoriesAsync();
}