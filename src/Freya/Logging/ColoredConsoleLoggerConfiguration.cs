using Microsoft.Extensions.Logging;

namespace Freya.Logging
{
    /// <summary>
    /// Represents the configuration for <see cref="ColoredConsoleLogger"/>.
    /// </summary>
    internal sealed class ColoredConsoleLoggerConfiguration
    {
        /// <summary>
        /// The highest <see cref="LogLevel"/> eligible to be logged.
        /// </summary>
        public LogLevel Threshold { get; set; } = LogLevel.Debug;
        /// <summary>
        /// A simple map between <see cref="LogLevel"/> to <see cref="ConsoleColor"/>.
        /// </summary>
        public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new()
        {
            [LogLevel.Trace] = ConsoleColor.Cyan,
            [LogLevel.Debug] = ConsoleColor.White,
            [LogLevel.Information] = ConsoleColor.Green,
            [LogLevel.Warning] = ConsoleColor.Yellow,
            [LogLevel.Error] = ConsoleColor.Red,
            [LogLevel.Critical] = ConsoleColor.Magenta
        };
    }
}
