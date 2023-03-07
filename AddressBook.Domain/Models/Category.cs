namespace AddressBook.Domain.Models;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? SubCategory { get; set; }
}