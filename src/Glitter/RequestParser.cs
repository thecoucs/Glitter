using Glitter.Commands;

using Mauve.Extensibility;
namespace Glitter;

/// <summary>
/// Represents a parser responsible for creating <see cref="CommandRequest"/> instances.
/// </summary>
public class RequestParser
{
    private readonly string _separator;
    private readonly string _commandToken;
    /// <summary>
    /// Creates a new <see cref="RequestParser"/> instance.
    /// </summary>
    /// <param name="commandToken">The token used to signify that the incoming message is a command.</param>
    public RequestParser(string commandToken, string separator)
    {
        _commandToken = commandToken;
        _separator = separator;
    }
    /// <summary>
    /// Attempts to parse the incoming message and generate a <see cref="CommandRequest"/>.
    /// </summary>
    /// <param name="message">The message to parse.</param>
    /// <param name="request">The resulting request.</param>
    /// <returns><see langword="true"/> if the message contains a command and is successfully parsed, otherwise <see langword="false"/>.</returns>
    /// TODO: Opt for SubstringQueue to handle the breakdown to prevent excess allocation of immutable resources.
    public bool TryParse(string message, out CommandRequest? request)
    {
        // If the message isn't considered a command, then there's no more work to do.
        request = null;
        if (message?.StartsWith(_commandToken) != true)
            return false;

        // Remove the command token from the message, break it down, and validate it.
        string rawInput = message.TakeAfter(ignoreCase: true, _commandToken);
        string key = rawInput.TakeTo(" ").Trim();
        if (string.IsNullOrWhiteSpace(key))
            return false;

        // Capture the remaining tokens.
        rawInput = rawInput.TakeAfter(" ");
        string[] tokens = rawInput.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
        if (tokens?.Any() != true)
            return false;

        // Create a new request and return true.
        request = new CommandRequest(key, tokens);
        return true;
    }
}
