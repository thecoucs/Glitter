using Discord;
using Discord.WebSocket;

using Freya.Core;
using Freya.Runtime;

using Mauve;
using Mauve.Extensibility;
using Mauve.Patterns;
using Mauve.Runtime.Processing;

namespace Freya.Services.Discord
{
    [Alias("discord")]
    internal class DiscordService : BotService<DiscordSettings>
    {

        #region Fields

        private readonly DiscordSocketClient _client;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public DiscordService(DiscordSettings settings, CancellationToken cancellationToken) :
            base(settings, new ConsoleLogger(), cancellationToken) =>
            _client = new DiscordSocketClient();

        #endregion

        #region Public Methods

        public override void Configure(IDependencyCollection dependencies, IPipeline<BotCommand> pipeline) =>
            _client.Log += DiscordLogReceived;

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Log(EventType.Information, "The Discord service is alive.");
        }

        #endregion

        #region Private Methods

        private async Task DiscordLogReceived(LogMessage arg)
        {
            // Determine the event type.
            EventType eventType = arg.Exception is null
                ? EventType.Information
                : EventType.Error;

            // Log the original message.
            if (!string.IsNullOrWhiteSpace(arg.Message))
                await Log(eventType, arg.Message);

            // Log the exception separately.
            if (arg.Exception is not null)
            {
                try
                {
                    await Log(EventType.Exception, arg.Exception.FlattenMessages(" "));
                } catch (Exception e)
                {
                    await Log(EventType.Exception, $"An unexpected error occurred while recording a log from Discord. {e.Message}");
                }
            }
        }

        #endregion

    }
}
