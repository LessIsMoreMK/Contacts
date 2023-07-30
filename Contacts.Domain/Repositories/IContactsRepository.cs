using Contacts.Domain.Entities;

namespace Contacts.Domain.Repositories;

/// <summary>
/// Repository for managing contacts.
/// </summary>
public interface IContactsRepository
{
    /// <summary>
    /// Returns all contacts from the database.
    /// </summary>
    /// <returns>An enumeration of Contact entities.</returns>
    public Task<IEnumerable<Contact>> GetAllContactsAsync();

    /// <summary>
    /// Finds and returns a contact using its unique identifier.
    /// </summary>
    /// <param name="contactId">The unique identifier of the contact.</param>
    /// <returns>A Contact entity, if found; otherwise, null.</returns>
    public Task<Contact?> GetContactByIdAsync(Guid contactId);
    
    /// <summary>
    /// Finds and returns a contact using its email.
    /// </summary>
    /// <param name="contactEmail">The email of the contact.</param>
    /// <returns>A Contact entity, if found; otherwise, null.</returns>
    public Task<Contact?> GetContactByEmailAsync(string contactEmail);
    
    /// <summary>
    /// Adds a new contact to the database.
    /// </summary>
    /// <param name="contact">The Contact entity to add.</param>
    public Task AddContactAsync(Contact contact);
    
    /// <summary>
    /// Updates an existing contact in the database.
    /// </summary>
    /// <param name="contact">The Contact entity for update.</param>
    public Task UpdateContactAsync(Contact contact);
    
    /// <summary>
    /// Deletes a contact from the database using its unique identifier.
    /// </summary>
    /// <param name="contactId">The unique identifier of the contact to delete.</param>
    public Task DeleteContactAsync(Guid contactId);
}
