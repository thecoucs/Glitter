using Discord;
using Discord.WebSocket;

using Freya.Core;
using Freya.Runtime;

using Mauve;
using Mauve.Extensibility;
using Mauve.Runtime.Processing;

using Microsoft.Extensions.DependencyInjection;

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
        public DiscordService(DiscordSettings settings, CommandFactory commandFactory, CancellationToken cancellationToken) :
            base("Discord", settings, new ConsoleLogger(), commandFactory, cancellationToken) =>
            _client = new DiscordSocketClient();

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override void ConfigureService(IServiceCollection services, IPipeline<BotCommand> pipeline)
        {
            _client.Log += HandleDiscordLog;
            _client.LoggedIn += HandleDiscordLogin;
            _client.LoggedOut += HandleClientLogout;
            _client.Connected += HandleClientConnect;
            _client.Disconnected += HandleClientDisconnect;
            _client.MessageReceived += HandleClientMessage;
            _ = services.AddSingleton(_client);
        }
        /// <inheritdoc/>
        protected override async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await _client.LoginAsync(TokenType.Bot, Settings.Token);
                await _client.StartAsync();
                await Log(EventType.Success, "The Discord service has been started successfully.");
            } catch (Exception e)
            {
                await Log(EventType.Exception, $"An unexpected error occurred while starting the Discord service. {e.Message}");
            }
        }

        #endregion

        #region Private Methods

        private async Task HandleClientConnect() =>
            await Log(EventType.Success, "Successfully connected to Discord.");
        private async Task HandleClientDisconnect(Exception arg) =>
            await Log(EventType.Exception, $"Disconnected from Discord. {arg.FlattenMessages(" ")}");
        private async Task HandleDiscordLogin() =>
            await Log(EventType.Success, "Logged in to Discord.");
        private async Task HandleClientLogout() =>
            await Log(EventType.Warning, "Logged out of Discord.");
        private async Task HandleDiscordLog(LogMessage arg)
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
        private async Task HandleClientMessage(SocketMessage arg) =>
            await Task.CompletedTask;

        #endregion

    }
}
