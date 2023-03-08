
using AddressBook.Application.Services.Authentication;
using AddressBook.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var authResult = await _authenticationService.Register(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );

        var response = new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.AccessToken,
            authResult.RefreshToken
        );

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var authResult = await _authenticationService.Login(
            request.Email,
            request.Password);

        var response = new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.AccessToken,
            authResult.RefreshToken
        );

        return Ok(response);
    }
}