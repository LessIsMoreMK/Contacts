using System.ComponentModel.DataAnnotations;

namespace Contacts.Infrastructure.Repositories.Postgres.Models;

public class ContactsDb
{
    #region Properties
    
    public Guid Id { get; set; }
    [Required] public string FirstName { get; set; } = null!;
    public string? LastName { get; set; } 
    public string? Email { get; set; } 
    public string? Phone { get; set; } 
    public DateTime? BirthDate { get; set; }
    
    public Guid CategoryId { get; set; }
    public virtual CategoriesDb Category { get; set; } = null!;
    
    #endregion
}