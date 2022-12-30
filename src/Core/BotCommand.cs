using Mauve;
using Mauve.Extensibility;
using Mauve.Math;
using Mauve.Runtime;

namespace Freya.Core
{
    internal abstract class BotCommand
    {

        #region Fields

        private readonly string _id;
        private readonly ILogger<LogEntry> _logger;

        #endregion

        #region Properties

        public string DisplayName { get; set; }
        public string Description { get; set; }

        #endregion

        #region Constructor

        public BotCommand(string displayName, string description, ILogger<LogEntry> logger)
        {
            _logger = logger;
            _id = Guid.NewGuid().GetHashCode(NumericBase.Hexadecimal);

            DisplayName = displayName;
            Description = description;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        /// <returns></returns>
        public async Task Execute(CancellationToken cancellationToken)
        {
            bool encounteredErrors = false;
            Log(EventType.Information, $"Executing command '{DisplayName}'...");
            try
            {
                _ = Work(cancellationToken);
            }
            catch (Exception e)
            {
                Log(EventType.Exception, $"An unexpected error occurred during execution. {e.Message}");
            }
            finally
            {
                // Set the event type for the completion message.
                EventType type = encounteredErrors
                    ? EventType.Error
                    : EventType.Success;

                // Log the completion message.
                Log(type, "Execution complete.");
                await Task.CompletedTask;
            }
        }

        #endregion

        #region Protected Methods

        protected abstract Task Work(CancellationToken cancellationToken);
        protected void Log(EventType eventType, string message) =>
            _logger.Log(new LogEntry(eventType, $"{_id}: {message}"));

        #endregion

    }
}