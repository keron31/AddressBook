using AddressBook.Domain.Models;

namespace AddressBook.Application.Common.Interfaces.Persistence;

public interface IContactRepository
{
    Contact? GetContactByEmail(string email);
    Task AddContactAsync(Contact contact);
    Task DeleteContactAsync(Contact contact);
    Task UpdateContactAsync(Contact contact);
    Task<Contact> GetContactByIdAsync(Guid id);
    Task<List<Contact>> GetContactsAsync();
}