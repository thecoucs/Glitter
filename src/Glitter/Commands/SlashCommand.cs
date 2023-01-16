using Microsoft.Extensions.Logging;

namespace Glitter.Commands;

/// <summary>
/// Represents a slash command that can be executed by a chatbot.
/// </summary>
public class SlashCommand : Command
{
    public SlashCommand(string key, string displayName, string description, ILogger logger) :
        base(key, displayName, description, logger)
    { }
    protected override Task<CommandResponse> Work(CancellationToken cancellationToken) => throw new NotImplementedException();
}
