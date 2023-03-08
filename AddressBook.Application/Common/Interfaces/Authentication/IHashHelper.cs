namespace AddressBook.Application.Common.Interfaces.Authentication;

public interface IHashHelper
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}