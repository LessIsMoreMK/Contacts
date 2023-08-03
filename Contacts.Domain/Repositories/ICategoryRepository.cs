using Contacts.Domain.Entities;

namespace Contacts.Domain.Repositories;

/// <summary>
/// Repository for managing categories.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Returns all categories along with their subcategories from the database.
    /// </summary>
    /// <returns>An enumeration of Category entities with their respective subcategories.</returns>
    public Task<IEnumerable<Category>> GetAllCategoriesWithSubcategoriesAsync();
    
    /// <summary>
    /// Finds and returns a category by name.
    /// </summary>
    /// <param name="categoryName">The name of the category.</param>
    /// <returns>A Category entity, if found; otherwise, null.</returns>
    public Task<Category?> GetCategoryByNameAsync(string categoryName);
    
    /// <summary>
    /// Finds and returns a category using its unique identifier.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category.</param>
    /// <returns>A Category entity, if found; otherwise, null.</returns>
    public Task<Category?> GetCategoryByIdAsync(Guid categoryId);
    
    /// <summary>
    /// Adds a new category to the database.
    /// </summary>
    /// <param name="category">The Category entity to add.</param>
    /// <returns>Created category GUID</returns>
    public Task<Guid> AddCategoryAsync(Category category);
}