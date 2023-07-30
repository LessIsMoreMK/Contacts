namespace Contacts.Infrastructure.Repositories.Postgres.Models;

public class CategoriesDb
{
    #region Properties
    
    public Guid Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<SubcategoriesDb> Subcategories { get; set; } = null!;
    
    #endregion
    
    #region Constructors

    public CategoriesDb()
    {
        
    }

    public CategoriesDb(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
    #endregion
}