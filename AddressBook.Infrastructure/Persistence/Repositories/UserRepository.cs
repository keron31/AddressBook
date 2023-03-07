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

    public void AddUser(User user)
    {
        _dbContext.Add(user);
        _dbContext.SaveChanges();
    }
}