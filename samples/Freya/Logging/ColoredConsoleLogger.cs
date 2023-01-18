namespace Freya.Logging;

/// <summary>
/// Represents a simple <see cref="ILogger"/> capable of recording logs in color.
/// </summary>
internal class ColoredConsoleLogger : ILogger
{
    private readonly string _categoryName;
    private readonly Dictionary<LogLevel, ConsoleColor> _logLevelToColorMap;
    /// <summary>
    /// Creates a new <see cref="ColoredConsoleLogger"/> instance.
    /// </summary>
    /// <param name="categoryName">The category name of the logger.</param>
    public ColoredConsoleLogger(string categoryName)
    {
        StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries |
                                          StringSplitOptions.TrimEntries;
        string[]? nameParts = categoryName?.Split('.', splitOptions);
        _categoryName = nameParts?.LastOrDefault() ?? "Unknown Logger";
        _logLevelToColorMap = new Dictionary<LogLevel, ConsoleColor>
        {
            [LogLevel.Trace] = ConsoleColor.Cyan,
            [LogLevel.Debug] = ConsoleColor.White,
            [LogLevel.Information] = ConsoleColor.Green,
            [LogLevel.Warning] = ConsoleColor.Yellow,
            [LogLevel.Error] = ConsoleColor.Red,
            [LogLevel.Critical] = ConsoleColor.Magenta
        };
    }
    /// <inheritdoc/>
    /// TODO: Research why this returns default! in the example.
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;
    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel) => true;
    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // Capture the incoming foreground color.
        ConsoleColor incomingColor = Console.ForegroundColor;
        try
        {
            // Set the foreground color and write the entry.
            Console.ForegroundColor = _logLevelToColorMap[logLevel];
            string? message = formatter?.Invoke(state, exception);
            Console.WriteLine($"{_categoryName}: {message ?? "Something went wrong trying to log this message."}");
        }
        catch { /* Gracefully ignore logging exceptions. */ }
        finally
        {
            // Reset the foreground color to the captured incoming color.
            Console.ForegroundColor = incomingColor;
        }
    }
}