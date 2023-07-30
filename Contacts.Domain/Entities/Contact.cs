using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace Contacts.Domain.Entities;

public partial class Contact
{
    #region Constants 
    
    [GeneratedRegex("^[\\w\\.-]+@[\\w\\.-]+\\.\\w+$")]
    private static partial Regex MyRegex();
    private static readonly Regex EmailRegex = MyRegex();
    
    #endregion
    
    #region Properties
    
    public Guid Id { get; private set; }
    public string FirstName { get; private set;} = null!;
    public string? LastName { get; private set; } 
    public string? Email { get; private set; } 
    public string? Phone { get; private set; } 
    public DateTime? BirthDate { get; private set; }
    public Guid CategoryId { get; private set; }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// Creates a contact instance after data validation.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <param name="birthDate"></param>
    /// <param name="categoryId"></param>
    /// <returns>Contact instance</returns>
    public static Contact Create(string firstName, string? lastName, string? email, string? phone, DateTime? birthDate, Guid categoryId)
    {
        if (string.IsNullOrEmpty(firstName))
            throw new ArgumentNullException(nameof(firstName));

        var contact = new Contact() {FirstName = firstName};
        
        if (!string.IsNullOrEmpty(email))
        {
            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException("Email is not valid", nameof(email));
            
            contact.Email = email;
        }

        contact.Id = Guid.NewGuid();
        contact.LastName = lastName;
        contact.Phone = phone;
        contact.BirthDate = birthDate;
        contact.CategoryId = categoryId;
        
        return contact;
    }
  
    #endregion

    /// <summary>
    /// Updates a contact instance after data validation.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <param name="birthDate"></param>
    /// <param name="categoryId"></param>
    public void Update(string firstName, string? lastName, string? email, string? phone, DateTime? birthDate, Guid categoryId)
    {
        if (string.IsNullOrEmpty(firstName))
            throw new ArgumentNullException(nameof(firstName));

        if (!string.IsNullOrEmpty(email))
        {
            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException("Email is not valid", nameof(email));
        }

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        BirthDate = birthDate;
        CategoryId = categoryId;
    }
}