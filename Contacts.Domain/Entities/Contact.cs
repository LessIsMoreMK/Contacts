namespace Contacts.Domain.Entities;

public class Contact
{
    #region Properties
    
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; } 
    public string? Email { get; set; } 
    public string? Phone { get; set; } 
    public DateTime? BirthDate { get; set; }
    
    #endregion
}