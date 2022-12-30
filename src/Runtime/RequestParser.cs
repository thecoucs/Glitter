using Freya.Commands;

using Mauve.Extensibility;
namespace Freya.Runtime
{
    internal class RequestParser
    {
        private readonly string _commandToken;
        public RequestParser(string commandToken) =>
            _commandToken = commandToken;
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

            // Capture the command key and any parameters.
            string? key = tokens.FirstOrDefault();
            foreach (string token in tokens.AfterOrDefault(key))
            {

            }

            return true;
        }
    }
}
