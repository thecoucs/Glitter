using Freya.Commands;

using Mauve.Extensibility;
namespace Freya.Runtime
{
    /// <summary>
    /// Represents a parser responsible for creating <see cref="CommandRequest"/> instances.
    /// </summary>
    internal class RequestParser
    {
        private readonly string _commandToken;
        /// <summary>
        /// Creates a new <see cref="RequestParser"/> instance.
        /// </summary>
        /// <param name="commandToken">The token used to signify that the incoming message is a command.</param>
        public RequestParser(string commandToken) =>
            _commandToken = commandToken;
        /// <summary>
        /// Attempts to parse the incoming message and generate a <see cref="CommandRequest"/>.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        /// <param name="request">The resulting request.</param>
        /// <returns><see langword="true"/> if the message contains a command and is successfully parsed, otherwise <see langword="false"/>.</returns>
        public bool TryParse(string message, out CommandRequest? request)
        {
            // If the message isn't considered a command, then there's no more work to do.
            request = null;
            if (message?.StartsWith(_commandToken) != true)
                return false;

            // Remove the command token from the message, break it down, and validate it.
            string rawInput = message.TakeAfter(ignoreCase: true, _commandToken);
            string[] tokens = rawInput.Split(' ');
            if (tokens?.Any() != true)
                return false;

            // Capture the command key and validate.
            string? key = tokens.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(key))
                return false;

            // Create a new request and return true.
            request = new CommandRequest(key, tokens.AfterOrDefault(key).ToArray());
            return true;
        }
    }
}
