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

        // Create JWT token
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);

        // Create refresh token
        var (refreshToken, refreshTokenExpiryDays) = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDate = refreshTokenExpiryDays;

        await _userRepository.AddUserAsync(user);

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
}