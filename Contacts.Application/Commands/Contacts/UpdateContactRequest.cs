namespace Contacts.Application.Commands.Contacts;

/// <summary>
/// Represents the request to update an existing contact.
/// </summary>
/// <param name="Id">The unique identifier of the contact to be updated.</param>
/// <param name="FirstName">The updated first name of the contact.</param>
/// <param name="LastName">The updated optional last name of the contact.</param>
/// <param name="Email">The updated optional email address of the contact.</param>
/// <param name="Phone">The updated optional phone number of the contact.</param>
/// <param name="BirthDate">The updated optional birth date of the contact.</param>
/// <param name="CategoryId">The updated ID of the category to which the contact belongs.</param>
public record UpdateContactRequest(
    Guid Id,
    string FirstName, 
    string? LastName, 
    string? Email, 
    string? Phone, 
    DateTime? BirthDate,
    Guid CategoryId);