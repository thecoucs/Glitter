using System.Text;

using Microsoft.Extensions.Logging;

namespace Glitter.Commands.OpenSource.Baseline;

/// <summary>
/// Represents a <see cref="SlashCommand"/> for getting the uptime of the bot.
/// </summary>
public sealed class UptimeCommand : SlashCommand
{
    private readonly SessionData _session;
    /// <summary>
    /// Creates a new <see cref="UptimeCommand"/> instance.
    /// </summary>
    /// <param name="logger">The logger the command should use.</param>
    public UptimeCommand(SessionData session, ILogger<UptimeCommand> logger) :
        base("uptime", "Get Uptime", "Queries the amount of time the bot has been up and running.", logger) =>
        _session = session;
    protected override Task<CommandResponse> Work(CancellationToken cancellationToken)
    {
        TimeSpan uptime = DateTime.Now - _session.BootDate;
        string uptimeString = GetUptimeString(uptime);
        return Task.FromResult(new CommandResponse($"The current uptime is {uptimeString}."));
    }
    private string GetUptimeString(TimeSpan uptime)
    {
        // Put the components into a dictionary for easy string building.
        Dictionary<string, int> components = new()
        {
            { "days", uptime.Days },
            { "hours", uptime.Hours },
            { "minutes", uptime.Minutes },
            { "seconds", uptime.Seconds },
        };

        // Build the output.
        // 1 day, 3 hours, 1 minute, and 52 seconds.
        int currentIndex = 0;
        var stringBuilder = new StringBuilder();
        foreach (string key in components.Keys)
        {
            // Ignore components with a zero value.
            int value = components[key];
            if (components[key] == 0)
            {
                currentIndex++;
                continue;
            }

            // Build out the current component.
            _ = stringBuilder.AppendIf(", ", stringBuilder.Length != 0)
                                .Append(value)
                                .AppendIf("and ", components.Skip(currentIndex++).Any(a => a.Value > 0))
                                .Append(value == 1 ? key.TrimEnd('s') : key);
        }

        return stringBuilder.ToString();
    }
}
