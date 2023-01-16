using Microsoft.Extensions.Logging;

namespace Glitter.Commands.OpenSource.General;

/// <summary>
/// Represents a <see cref="Command"/> for getting the current version of the bot.
/// </summary>
public sealed class VersionCommand : Command
{
    /// <summary>
    /// Creates a new <see cref="VersionCommand"/> instance.
    /// </summary>
    /// <param name="logger">The logger for the command to use.</param>
    public VersionCommand(ILogger<VersionCommand> logger) :
        base("version", "Version", "Gets the current version of the bot.", logger)
    { }
    protected override async Task<CommandResponse> Work(CancellationToken cancellationToken)
    {
        var response = new CommandResponse($"🐣 Pre-Release");
        return await Task.FromResult(response);
    }
}
