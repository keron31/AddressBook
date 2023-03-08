using AddressBook.Application.Common.Interfaces.Persistence;
using AddressBook.Domain.Models;

namespace AddressBook.Application.Services.Contacts;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<ContactResult> CreateContact(string FirstName,
        string LastName,
        string Email,
        string Password,
        string Category,
        string? SubCategory,
        string Phone,
        DateTime DateOfBirth)
    {
        if (_contactRepository.GetContactByEmail(Email) is not null)
        {
            throw new Exception("Email already in use");
        }

        var contact = new Contact
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password,
            Category = Category,
            SubCategory = SubCategory,
            Phone = Phone,
            DateOfBirth = DateOfBirth
        };

        await _contactRepository.AddContactAsync(contact);

        return new ContactResult(contact);
    }
}