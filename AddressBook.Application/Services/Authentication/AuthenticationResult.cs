using AddressBook.Domain.Models;

namespace AddressBook.Application.Services.Authentication;

public record AuthenticationResult(
    User User,
    string Token);