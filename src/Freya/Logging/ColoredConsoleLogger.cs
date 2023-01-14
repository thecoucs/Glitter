using Microsoft.Extensions.Logging;

namespace Freya.Logging
{
    /// <summary>
    /// Represents a simple <see cref="ILogger"/> capable of recording logs in color.
    /// </summary>
    internal class ColoredConsoleLogger : ILogger
    {
        private readonly string _name;
        private readonly Func<ColoredConsoleLoggerConfiguration> _configurationQueryFunction;
        /// <summary>
        /// Creates a new <see cref="ColoredConsoleLogger"/> instance.
        /// </summary>
        /// <param name="configurationQueryFunction">A function that returns the current <see cref="ColoredConsoleLoggerConfiguration"/>.</param>
        public ColoredConsoleLogger(string name, Func<ColoredConsoleLoggerConfiguration> configurationQueryFunction)
        {
            _name = name;
            _configurationQueryFunction = configurationQueryFunction;
        }
        /// <inheritdoc/>
        /// TODO: Research why this returns default! in the example.
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;
        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel) =>
            TryGetCurrentConfiguration(out ColoredConsoleLoggerConfiguration? configuration) &&
            configuration?.Threshold > logLevel;
        /// <inheritdoc/>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // If the current log level isn't allowed or we can't load the configuration, then don't log.
            if (!IsEnabled(logLevel) || // TODO: Configuration is loaded twice back-to-back.
                !TryGetCurrentConfiguration(out ColoredConsoleLoggerConfiguration? configuration) &&
                configuration is not null)
                return;

            // Capture the incoming foreground color.
            ConsoleColor incomingColor = Console.ForegroundColor;
            try
            {
                // Set the foreground color and write the entry.
                Console.ForegroundColor = configuration?.LogLevelToColorMap[logLevel] ?? incomingColor;
                Console.WriteLine($"{_name}: {formatter?.Invoke(state, exception)}");
            } catch { /* Gracefully ignore logging exceptions. */ } finally
            {
                // Reset the foreground color to the captured incoming color.
                Console.ForegroundColor = incomingColor;
            }
        }
        private bool TryGetCurrentConfiguration(out ColoredConsoleLoggerConfiguration? configuration)
        {
            try
            {
                configuration = _configurationQueryFunction();
                return true;
            } catch { /* Gracefully ignore logging exceptions. */ }

            configuration = null;
            return false;
        }
    }
}
