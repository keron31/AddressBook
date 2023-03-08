using AddressBook.Domain.Models;

namespace AddressBook.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
}