namespace Contacts.Infrastructure.Repositories.Postgres.Models;

public class SubcategoriesDb
{
    #region Properties
    
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public Guid CategoryId { get; set; }
    
    public virtual CategoriesDb Category { get; set; }
    
    #endregion
    
    #region Constructors

    public SubcategoriesDb()
    {
    }

    public SubcategoriesDb(Guid id, string name, Guid categoryId)
    {
        Id = id;
        Name = name;
        CategoryId = categoryId;
    }
    
    #endregion
}