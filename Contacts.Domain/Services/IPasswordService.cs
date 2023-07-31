namespace Contacts.Domain.Services;

public interface IPasswordService
{
    /// <summary>
    /// Creates a random salt.
    /// </summary>
    /// <returns>Salt</returns>
    string CreateSalt();
    
    /// <summary>
    /// Hashes the password with the salt using SHA256.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    string HashPassword(string password, string salt);

    /// <summary>
    /// Verifies if the provided password matches the hashed version.
    /// </summary>
    /// <param name="providedPassword"></param>
    /// <param name="hashedPassword"></param>
    /// <param name="salt"></param>
    /// <returns>True if matched; false otherwise</returns>
    bool VerifyPassword(string providedPassword, string hashedPassword, string salt);
}