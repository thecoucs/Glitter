using Mauve.Extensibility;
using Mauve.Math;

using Microsoft.Extensions.Logging;

namespace Freya.Commands
{
    internal abstract class Command
    {
        private readonly string _id;
        private readonly ILogger _logger;
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Command(string displayName, string description, ILogger logger)
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
            Log(LogLevel.Information, $"Executing command '{DisplayName}'...");
            try
            {
                response = await Work(cancellationToken);
            } catch (Exception e)
            {
                Log(LogLevel.Error, $"An unexpected error occurred during execution. {e.Message}");
            } finally
            {
                // Set the event type for the completion message.
                LogLevel logLevel = encounteredErrors
                    ? LogLevel.Error
                    : LogLevel.Information;

                // Log the completion message.
                Log(logLevel, "Execution complete.");
            }

            return await Task.FromResult(response);
        }
        protected abstract Task<CommandResponse> Work(CancellationToken cancellationToken);
        protected void Log(LogLevel level, string message) =>
            _logger.Log(level, $"{_id}: {message}");
    }
}