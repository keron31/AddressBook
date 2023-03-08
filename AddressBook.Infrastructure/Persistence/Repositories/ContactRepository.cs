using AddressBook.Application.Common.Interfaces.Persistence;
using AddressBook.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AddressBookDbContext _dbContext;

    public ContactRepository(AddressBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Contact? GetContactByEmail(string email)
    {
        return _dbContext.Contacts.FirstOrDefault(c => c.Email == email);
    }

    public async Task AddContactAsync(Contact contact)
    {
        await _dbContext.Contacts.AddAsync(contact);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(Contact contact)
    {
        _dbContext.Contacts.Remove(contact);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateContactAsync(Contact contact)
    {
        _dbContext.Contacts.Update(contact);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Contact> GetContactByIdAsync(Guid id)
    {
        return await _dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync()
    {
        return await _dbContext.Contacts.ToListAsync();

    }
}