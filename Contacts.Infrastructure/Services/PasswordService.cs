using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Contacts.Domain.Entities;
using Contacts.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Contacts.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    #region Fields

    public readonly IConfiguration _configuration;
    
    #endregion
    
    #region Constructor

    public PasswordService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    #endregion
    
    #region Methods
    
    public string CreateSalt()
    {
        var buffer = new byte[16];
        RandomNumberGenerator.Fill(buffer);
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
    
    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("jwt:key")!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    #endregion
}