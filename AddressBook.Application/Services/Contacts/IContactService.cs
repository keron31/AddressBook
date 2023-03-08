namespace AddressBook.Application.Services.Contacts;

public interface IContactService
{
    Task<ContactResult> CreateContact(string FirstName,
        string LastName,
        string Email,
        string Password,
        string Category,
        string? SubCategory,
        string Phone,
        DateTime DateOfBirth
    );
}