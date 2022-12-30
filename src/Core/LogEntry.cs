using Mauve;

namespace Freya.Core
{
    /// <summary>
    /// Represents a log entry.
    /// </summary>
    internal record LogEntry
    {

        #region Properties

        /// <summary>
        /// The <see cref="EventType"/> of the entry.
        /// </summary>
        public EventType Type { get; init; }
        /// <summary>
        /// The message the entry is for.
        /// </summary>
        public string Message { get; init; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="LogEntry"/>.
        /// </summary>
        /// <param name="type">The <see cref="EventType"/> of the entry.</param>
        /// <param name="message">The message the entry is for.</param>
        public LogEntry(EventType type, string message)
        {
            Type = type;
            Message = message;
        }

        #endregion

    }
}
