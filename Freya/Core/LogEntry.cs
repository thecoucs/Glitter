using Mauve;

namespace Freya.Core
{
    internal class LogEntry
    {

        #region Properties

        public EventType Type { get; set; }
        public string Message { get; set; }

        #endregion

        #region Constructor

        public LogEntry() :
            this(EventType.None, string.Empty)
        { }
        public LogEntry(EventType type, string message)
        {
            Type = type;
            Message = message;
        }

        #endregion

    }
}
