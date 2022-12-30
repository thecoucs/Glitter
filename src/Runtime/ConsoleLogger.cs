using Freya.Core;

using Mauve;
using Mauve.Runtime;

namespace Freya.Runtime
{
    /// <summary>
    /// Represents an <see cref="ILogger{T}"/> implementation focused on writing <see cref="LogEntry"/>s to the <see cref="Console"/>.
    /// </summary>
    internal class ConsoleLogger : ILogger<LogEntry>
    {
        /// <summary>
        /// Logs the specified <see cref="LogEntry"/> to the <see cref="Console"/>.
        /// </summary>
        /// <param name="input">The log entry to record.</param>
        public void Log(LogEntry input)
        {
            // Capture the incoming foreground color.
            ConsoleColor incomingColor = Console.ForegroundColor;
            try
            {
                // Write the message.
                Console.ForegroundColor = GetForecolor(input.Type, incomingColor);
                Console.WriteLine(input.Message);
            } catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An unexpected error occurred while logging. {e.Message}");
            } finally
            {
                Console.ForegroundColor = incomingColor;
            }
        }
        /// <inheritdoc/>
        public async Task LogAsync(LogEntry input) =>
            await LogAsync(input, CancellationToken.None);
        /// <inheritdoc/>
        public async Task LogAsync(LogEntry input, CancellationToken cancellationToken) =>
            await Task.Run(() => Log(input), cancellationToken);
        /// <summary>
        /// Gets the <see cref="ConsoleColor"/> to use for the specified <see cref="EventType"/>.
        /// </summary>
        /// <param name="type">The <see cref="EventType"/> being recorded.</param>
        /// <param name="defaultColor">The default <see cref="ConsoleColor"/> that should be utilized if no conditions are met.</param>
        /// <returns>The <see cref="ConsoleColor"/> that should be utilized when writing the <see cref="LogEntry"/>.</returns>
        private ConsoleColor GetForecolor(EventType type, ConsoleColor defaultColor) => type.HasFlag(EventType.None) switch
        {
            true when type.HasFlag(EventType.Success) => ConsoleColor.Green,
            true when type.HasFlag(EventType.Warning) => ConsoleColor.Yellow,
            true when type.HasFlag(EventType.Error) || type.HasFlag(EventType.Exception) => ConsoleColor.Red,
            _ => defaultColor
        };
    }
}
