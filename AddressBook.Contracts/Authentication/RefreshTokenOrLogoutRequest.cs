namespace AddressBook.Contracts.Authentication;

public record RefreshTokenOrLogoutRequest(
    string AccessToken,
    string RefreshToken
);