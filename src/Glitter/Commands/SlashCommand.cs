using Microsoft.Extensions.Logging;

namespace Glitter.Commands;

/// <summary>
/// Represents a slash command that can be executed by a chatbot.
/// </summary>
public abstract class SlashCommand : Command
{
    /// <summary>
    /// Creates a new <see cref="SlashCommand"/> instance.
    /// </summary>
    /// <param name="key">The key for invoking the command.</param>
    /// <param name="displayName">The display name for the command.</param>
    /// <param name="description">The description for the command.</param>
    /// <param name="logger">The logger the command should utilize.</param>
    public SlashCommand(string key, string displayName, string description, ILogger logger) :
        base(key, displayName, description, logger)
    { }
}
