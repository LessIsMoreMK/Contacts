using Microsoft.Extensions.Logging;

namespace Contacts.Application.Helpers;

/// <summary>
/// LoggerHelpers is a utility class used for logging throughout the application.
/// </summary>
public class LoggerHelpers
{
    private readonly ILogger<LoggerHelpers> _logger;

    public LoggerHelpers(ILogger<LoggerHelpers> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs an error message and exception.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    /// <param name="message">The error message to log.</param>
    public void LogError(System.Exception ex, string message)
        => _logger.LogError(ex, message);
}