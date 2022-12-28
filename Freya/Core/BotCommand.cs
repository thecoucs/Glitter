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

        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        #endregion

        #region Constructor

        public BotCommand(string key, string displayName, string description, ILogger<LogEntry> logger)
        {
            _logger = logger;
            _id = Guid.NewGuid().GetHashCode(NumericBase.Hexadecimal);

            Key = key;
            DisplayName = displayName;
            Description = description;
        }

        #endregion

        #region Public Methods

        public void Execute()
        {
            bool encounteredErrors = false;
            Log(EventType.Information, $"Executing command '{DisplayName}'...");
            try
            {
                Work();
            } catch (Exception e)
            {
                Log(EventType.Exception, $"An unexpected exception occurred during execution. {e.Message}");
            } finally
            {
                // Set the event type for the completion message.
                EventType type = encounteredErrors
                    ? EventType.Error
                    : EventType.Success;

                // Log the completion message.
                Log(type, "Execution complete.");
            }
        }

        #endregion

        #region Protected Methods

        protected abstract void Work();
        protected void Log(EventType eventType, string message) =>
            _logger.Log(new LogEntry(eventType, $"{_id}: {message}"));

        #endregion

    }
}