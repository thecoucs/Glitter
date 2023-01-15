namespace Glittertind.Commands.OpenSource
{
    /// <summary>
    /// Represents a collection of session data for commands to utilize.
    /// </summary>
    public class SessionData
    {
        /// <summary>
        /// The date and time the service booted up.
        /// </summary>
        public DateTime BootDate { get; private set; }
        /// <summary>
        /// Creates a new <see cref="SessionData"/> instance.
        /// </summary>
        public SessionData() =>
            BootDate = DateTime.Now;
    }
}
