using MediatR;

namespace Glittertind.Commands.OpenSource
{
    /// <summary>
    /// Represents a request for the uptime of the bot.
    /// </summary>
    public class UptimeRequest : IRequest<TimeSpan>
    {

    }
}
