namespace Contacts.Application.Dtos;

/// <summary>
/// Represents a simpler version of the Contact entity.
/// </summary>
/// <param name="Id">Contact internal id</param>
/// <param name="FirstName">FirstName</param>
/// <param name="LastName">LastName</param>
public record ContactSimpleDto(Guid Id, string FirstName, string? LastName);