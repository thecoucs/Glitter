namespace Glitter.Commands
{
    /// <summary>
    /// Represents a <see cref="CommandResponse"/>.
    /// </summary>
    public sealed class CommandResponse
    {
        /// <summary>
        /// The message for the response.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Creates a new instance of <see cref="CommandResponse"/>.
        /// </summary>
        /// <param name="message">The message for the response.</param>
        public CommandResponse(string message) =>
            Message = message;
    }
}
