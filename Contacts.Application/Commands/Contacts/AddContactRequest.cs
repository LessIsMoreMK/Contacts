namespace Contacts.Application.Commands.Contacts;

/// <summary>
/// Represents the request to add a new contact.
/// </summary>
/// <param name="FirstName">The first name of the contact.</param>
/// <param name="LastName">The optional last name of the contact.</param>
/// <param name="Email">The optional email address of the contact.</param>
/// <param name="Phone">The optional phone number of the contact.</param>
/// <param name="BirthDate">The optional birth date of the contact.</param>
/// <param name="CategoryId">The ID of the category to which the contact belongs.</param>
public record AddContactRequest(
    string FirstName, 
    string? LastName, 
    string? Email, 
    string? Phone, 
    DateTime? BirthDate,
    Guid CategoryId);