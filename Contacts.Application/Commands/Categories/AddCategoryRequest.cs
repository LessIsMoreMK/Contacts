namespace Contacts.Application.Commands.Categories;

/// <summary>
/// Represents the request to add a new category.
/// </summary>
/// <param name="Name">The name of the new category to be added.</param>
public record AddCategoryRequest(string Name);