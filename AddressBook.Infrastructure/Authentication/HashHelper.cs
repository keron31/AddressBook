using System.Security.Cryptography;
using AddressBook.Application.Common.Interfaces.Authentication;

namespace AddressBook.Infrastructure.Authentication;

public class HashHelper : IHashHelper
{
    private const int SaltSize = 16;
    private const int HashSize = 20;
    private const int HashIter = 100000;
    public string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Generate the hash using PBKDF2 with 10000 iterations and SHA256 algorithm
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, HashIter, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Combine the salt and hash into a single byte array
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        // Return the hash as a Base64-encoded string
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        // Decode the Base64-encoded hash string and extract the salt
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        // Generate the hash using the password and the extracted salt
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, HashIter, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Compare the generated hash with the stored hash
        for (int i = 0; i < 20; i++)
        {
            if (hashBytes[i + 16] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}