namespace Contacts.Domain.Entities;

public class Category
{
    #region Properties
    
    public Guid Id { get; private set; }
    
    public string Name { get; private set; } = null!;

    public ICollection<Subcategory> Subcategories { get; set; } = null!;
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// Creates a category instance after data validation.
    /// </summary>
    /// <param name="name">Category name</param>
    /// <returns>Category instance</returns>
    public static Category Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        var category = new Category()
        {
            Id = Guid.NewGuid(), 
            Name = name
        };        
        
        return category;
    }

    #endregion
}