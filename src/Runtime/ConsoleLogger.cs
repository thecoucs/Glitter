using Freya.Core;
using Mauve;
using Mauve.Runtime;

namespace Freya.Runtime
{
    internal class ConsoleLogger : ILogger<LogEntry>
    {

        #region Public Methods

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
        public async Task LogAsync(LogEntry input) =>
            await LogAsync(input, CancellationToken.None);
        public async Task LogAsync(LogEntry input, CancellationToken cancellationToken) =>
            await Task.Run(() => Log(input), cancellationToken);

        #endregion

        #region Private Methods

        private ConsoleColor GetForecolor(EventType type, ConsoleColor defaultColor) => type.HasFlag(EventType.None) switch
        {
            true when type.HasFlag(EventType.Success) => ConsoleColor.Green,
            true when type.HasFlag(EventType.Warning) => ConsoleColor.Yellow,
            true when type.HasFlag(EventType.Error) || type.HasFlag(EventType.Exception) => ConsoleColor.Red,
            _ => defaultColor
        };

        #endregion

    }
}
