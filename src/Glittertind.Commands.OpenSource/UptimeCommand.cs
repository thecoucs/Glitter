using MediatR;

using Microsoft.Extensions.Logging;

namespace Glittertind.Commands.OpenSource
{
    /// <summary>
    /// Represents a <see cref="Command"/> for getting the uptime of the bot.
    /// </summary>
    public sealed class UptimeCommand : Command
    {
        /// <summary>
        /// Creates a new <see cref="UptimeCommand"/> instance.
        /// </summary>
        /// <param name="logger">The logger the command should use.</param>
        public UptimeCommand(IMediator mediator, ILogger<UptimeCommand> logger) :
            base("uptime", "Get Uptime", "Queries the amount of time the bot has been up and running.", mediator, logger)
        { }
        protected override async Task<CommandResponse> Work(CancellationToken cancellationToken)
        {
            var request = new UptimeRequest();
            TimeSpan uptime = await Mediator.Send(request);
            return new CommandResponse($"The current uptime is {uptime}.");
        }
    }
}
