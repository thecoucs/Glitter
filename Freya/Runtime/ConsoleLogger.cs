using Freya.Core;

using Mauve;
using Mauve.Runtime;

namespace Freya.Runtime
{
    internal class ConsoleLogger : ILogger<LogEntry>
    {
        public void Log(LogEntry input)
        {
            // Capture the incoming foreground color.
            ConsoleColor incomingColor = Console.ForegroundColor;

            try
            {
                // Determine the color we should write with.
                ConsoleColor writeColor = input.Type.HasFlag(EventType.Success)
                    ? ConsoleColor.Green
                    : input.Type.HasFlag(EventType.Warning)
                        ? ConsoleColor.Yellow
                        : input.Type.HasFlag(EventType.Error) ||
                            input.Type.HasFlag(EventType.Exception)
                            ? ConsoleColor.Red
                            : incomingColor;

                // Write the message.
                Console.ForegroundColor = writeColor;
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
    }
}
