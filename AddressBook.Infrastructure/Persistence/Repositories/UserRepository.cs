using AddressBook.Application.Common.Interfaces.Persistence;
using AddressBook.Domain.Models;

namespace AddressBook.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AddressBookDbContext _dbContext;

    public UserRepository(AddressBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User? GetUserByEmail(string email)
    {
        return _dbContext.Users.SingleOrDefault(x => x.Email == email);
    }

    public async Task<User> AddUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
}