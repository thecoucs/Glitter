using Mauve.Runtime;

namespace Freya.Core
{
    internal abstract class BotCommand
    {

        #region Fields

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
            Key = key;
            DisplayName = displayName;
            Description = description;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public abstract void Execute();

        #endregion

        #region Protected Methods

        #endregion

    }
}