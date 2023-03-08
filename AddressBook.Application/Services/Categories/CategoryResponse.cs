namespace AddressBook.Application.Services.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    List<string> SubCategoryNames
);