using System.Security.Cryptography;
using System.Text;
using Contacts.Domain.Services;

namespace Contacts.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    #region Methods
    
    public string CreateSalt()
    {
        var rng = new RNGCryptoServiceProvider();
        var buffer = new byte[16];
        rng.GetBytes(buffer);
        return Convert.ToBase64String(buffer);
    }

    public string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var saltedPassword = string.Concat(salt, password);
        var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
        return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
    }

    public bool VerifyPassword(string providedPassword, string hashedPassword, string salt)
    {
        var hashOfProvidedPassword = HashPassword(providedPassword, salt);
        return hashedPassword == hashOfProvidedPassword;
    }
    
    #endregion
}