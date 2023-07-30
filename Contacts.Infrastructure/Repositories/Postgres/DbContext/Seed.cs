using Contacts.Infrastructure.Repositories.Postgres.Models;

namespace Contacts.Infrastructure.Repositories.Postgres.DbContext;

/// <summary>
/// The Seed class provides methods to generate predefined data for the application.
/// This data is used to seed the database with initial data during the database initialization process.
/// </summary>
internal static class Seed
{
    #region Methods

    /// <summary>
    /// Returns an array of predefined categories.
    /// </summary>
    /// <returns>An array of CategoriesDb instances.</returns>
    public static CategoriesDb[] GetCategories()
    {
        return new []
        {
            new CategoriesDb(new Guid("04280a09-fc05-4e53-ae32-29f991dca066"), "Private"),
            new CategoriesDb(new Guid("1a95a6d6-8e45-41db-b149-d9d79f23c392"), "Business"),
            new CategoriesDb(new Guid("31144dff-a3ce-4c00-ad5a-2c2e06f7dd8b"), "Other")
        };
    }
    
    /// <summary>
    /// Returns an array of predefined subcategories.
    /// </summary>
    /// <returns>An array of SubcategoriesDb instances.</returns>
    public static SubcategoriesDb[] GetSubcategories()
    {
        var businessCategoryId = new Guid("1a95a6d6-8e45-41db-b149-d9d79f23c392");
        
        return new []
        {
            new SubcategoriesDb(new Guid("f6ea6c6b-aff3-487f-b7f2-b76af18a44d9"), "Boss", businessCategoryId),
            new SubcategoriesDb(new Guid("f4aa8bb8-9b1f-478a-9f94-f9b7c367a6e3"), "Client", businessCategoryId)
        };
    }

    #endregion
}