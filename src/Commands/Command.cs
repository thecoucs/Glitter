using Freya.Core;

using Mauve;
using Mauve.Extensibility;
using Mauve.Math;
using Mauve.Runtime;

namespace Freya.Commands
{
    internal abstract class Command
    {
        private readonly string _id;
        private readonly ILogger<LogEntry> _logger;
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Command(string displayName, string description, ILogger<LogEntry> logger)
        {
            _logger = logger;
            _id = Guid.NewGuid().GetHashCode(NumericBase.Hexadecimal);

            DisplayName = displayName;
            Description = description;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        /// <returns></returns>
        public async Task<CommandResponse?> Execute(CancellationToken cancellationToken)
        {
            bool encounteredErrors = false;
            CommandResponse? response = null;
            Log(EventType.Information, $"Executing command '{DisplayName}'...");
            try
            {
                response = await Work(cancellationToken);
            } catch (Exception e)
            {
                Log(EventType.Exception, $"An unexpected error occurred during execution. {e.Message}");
            } finally
            {
                // Set the event type for the completion message.
                EventType type = encounteredErrors
                    ? EventType.Error
                    : EventType.Success;

                // Log the completion message.
                Log(type, "Execution complete.");
            }

            return await Task.FromResult(response);
        }
        protected abstract Task<CommandResponse> Work(CancellationToken cancellationToken);
        protected void Log(EventType eventType, string message) =>
            _logger.Log(new LogEntry(eventType, $"{_id}: {message}"));
    }
}