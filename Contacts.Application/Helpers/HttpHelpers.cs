using System.Net;
using Microsoft.AspNetCore.Http;

namespace Contacts.Application.Helpers;

/// <summary>
/// HttpHelpers is a static utility class for HTTP response-related operations.
/// </summary>
public static class HttpHelpers
{
    /// <summary>
    /// Set the HTTP response status code and message.
    /// </summary>
    /// <param name="context">The HttpContext instance.</param>
    /// <param name="statusCode">The HttpStatusCode to set for the response.</param>
    /// <param name="message">The message to include in the response body.</param>
    public static async Task SetResponseAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(new { Message = message });
    }
}