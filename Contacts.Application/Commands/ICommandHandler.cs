using Microsoft.AspNetCore.Http;

namespace Contacts.Application.Commands;

/// <summary>
/// Represents a command handler in a CQRS pattern.
/// </summary>
/// <typeparam name="TCommand">The type of command this handler can process.</typeparam>
public interface ICommandHandler<in TCommand>
{
    /// <summary>
    /// Handles a command asynchronously. This method should contain the logic for processing a command.
    /// </summary>
    /// <param name="command">The command to be processed.</param>
    /// <param name="context">The HttpContext instance for the current HTTP request.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleAsync(TCommand command, HttpContext context);
}