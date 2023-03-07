using AddressBook.Domain.Models;

namespace AddressBook.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    void AddUser(User user);
}