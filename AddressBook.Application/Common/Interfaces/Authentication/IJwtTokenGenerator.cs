using System.Security.Claims;
using AddressBook.Domain.Models;

namespace AddressBook.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken (User user);
    (string refreshToken, DateTime RefreshTokenExpiryDays) GenerateRefreshToken ();
    ClaimsPrincipal GetPrincipalFromExpiredToken (string token);
}