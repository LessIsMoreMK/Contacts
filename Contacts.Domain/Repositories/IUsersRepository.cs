using Contacts.Domain.Entities;

namespace Contacts.Domain.Repositories;

public interface IUsersRepository
{
    /// <summary>
    /// Finds and returns a user using its userName.
    /// </summary>
    /// <param name="userName">The userName to find.</param>
    /// <returns>A User entity, if found; otherwise, null.</returns>
    public Task<User?> GetUserByUserNameAsync(string userName);
    
    /// <summary>
    /// Adds a new user to the database.
    /// </summary>
    /// <param name="user">The User entity to add.</param>
    public Task AddUserAsync(User user);
}