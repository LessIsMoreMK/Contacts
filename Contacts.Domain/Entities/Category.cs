namespace Contacts.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;

    public ICollection<Subcategory> Subcategories { get; set; } = null!;
}