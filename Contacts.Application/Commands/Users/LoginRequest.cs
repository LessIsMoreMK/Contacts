namespace Contacts.Application.Commands.Users;

/// <summary>
/// Represents a request to create a new user.
/// </summary>
/// <param name="UserName"></param>
/// <param name="Password"></param>
public record LoginRequest(string UserName, string Password);