using Microsoft.Extensions.Logging;

namespace Freya
{
    /// <summary>
    /// Represents a basic event handler.
    /// </summary>
    public abstract class EventHandler
    {
        /// <summary>
        /// The logger for the service the event is related to.
        /// </summary>
        public ILogger Logger { get; private set; }
        /// <summary>
        /// Creates a new <see cref="EventHandler"/> instance.
        /// </summary>
        /// <param name="logger">The logger for the service the event is related to.</param>
        public EventHandler(ILogger logger) =>
            Logger = logger;
    }
}