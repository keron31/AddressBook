namespace AddressBook.Domain.Models;

public class Contact
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? SubCategory { get; set; }
    public string Phone { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
}