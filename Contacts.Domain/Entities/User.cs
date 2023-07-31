using System.Text.RegularExpressions;

namespace Contacts.Domain.Entities;

//NOTE: Properties are public for Builder pattern used it tests
public partial class User
{
    #region Constants
    
    // Password requires at least 8 characters, with a mix of uppercase, lowercase, digits and special character.
    private static readonly Regex PasswordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$");
    
    #endregion
    
    #region Properties
    
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public string Salt { get; set; } = null!;
    
    #endregion
    
    #region Methods

    /// <summary>
    /// Creates a user instance after data validation.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns>User instance.</returns>
    public static User Create(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName))
            throw new ArgumentNullException(nameof(userName));
        
        if (string.IsNullOrEmpty(password) || !PasswordRegex.IsMatch(password))
            throw new ArgumentException("Password requires at least 8 characters, with a mix of uppercase, lowercase, digits and special character", nameof(password));
        
        var user = new User()
        {
            Id = Guid.NewGuid(), 
            UserName = userName, 
            Password = password
        };

        return user;
    }
    
    #endregion
}