using System.Security.Claims;
using AddressBook.Application.Common.Interfaces.Authentication;
using AddressBook.Application.Common.Interfaces.Persistence;
using AddressBook.Domain.Models;

namespace AddressBook.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IHashHelper _hashHelper;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IHashHelper hashHelper)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _hashHelper = hashHelper;
    }

    public async Task<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            throw new Exception("Email already in use");
        }

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = _hashHelper.HashPassword(password)
        };

        await _userRepository.AddUserAsync(user);

        // Create JWT token
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);

        // Create refresh token
        var (refreshToken, refreshTokenExpiryDays) = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDate = refreshTokenExpiryDays;

        await _userRepository.UpdateUserAsync(user);

        return new AuthenticationResult(
            user,
            accessToken,
            refreshToken);
    }

    public async Task<AuthenticationResult> Login(string email, string password)
    {
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            throw new Exception("Invalid credentials");
        }

        if (!_hashHelper.VerifyPassword(password, user.Password))
        {
            throw new Exception("Invalid credentials");
        }

        // Create JWT token
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);

        // Create refresh token
        var (refreshToken, refreshTokenExpiryDays) = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDate = refreshTokenExpiryDays;

        await _userRepository.UpdateUserAsync(user);

        return new AuthenticationResult(
            user,
            accessToken,
            refreshToken);
    }

    public async Task<AuthenticationResult> RefreshToken (string accessToken, string refreshToken)
    {
        var validatedToken = _jwtTokenGenerator.GetPrincipalFromExpiredToken(accessToken);

        if (validatedToken is null)
        {
            throw new Exception("Invalid token");
        }

        var email = validatedToken.Claims.First(c => c.Type == ClaimTypes.Email).Value;

        if (email is null)
        {
            throw new Exception("Invalid token");
        }

        var user = _userRepository.GetUserByEmail(email);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
        {
            throw new Exception("Invalid refresh token or token expired or user not found");
        }

        var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var (newRefreshToken, newRefreshTokenExpiryDays) = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryDate = newRefreshTokenExpiryDays;

        await _userRepository.UpdateUserAsync(user);

        return new AuthenticationResult(
            user,
            newAccessToken,
            newRefreshToken);

    }

    public async Task Logout(string accessToken, string refreshToken)
    {
        var validatedToken = _jwtTokenGenerator.GetPrincipalFromExpiredToken(accessToken);

        if (validatedToken is null)
        {
            throw new Exception("Invalid token");
        }

        var email = validatedToken.Claims.First(c => c.Type == ClaimTypes.Email).Value;

        if (email is null)
        {
            throw new Exception("Invalid token");
        }

        var user = _userRepository.GetUserByEmail(email);

        if (user is null || user.RefreshToken != refreshToken)
        {
            throw new Exception("Invalid refresh token or user not found");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryDate = null;

        await _userRepository.UpdateUserAsync(user);
    }
}