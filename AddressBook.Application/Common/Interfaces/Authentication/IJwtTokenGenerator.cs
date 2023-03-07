using AddressBook.Domain.Models;

namespace AddressBook.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}