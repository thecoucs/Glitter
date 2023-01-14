using Discord;
using Discord.WebSocket;

using Freya.Core;
using Freya.Services;

using Mauve;
using Mauve.Extensibility;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Freya.Providers.Discord
{
    /// <summary>
    /// Represents a new <see cref="Chatbot"/> for integrating with <see href="https://discordnet.dev/guides/introduction/intro.html">Discord</see>.
    /// </summary>
    internal class DiscordChatbot : Chatbot<DiscordSettings>
    {
        private readonly DiscordSocketClient _client;
        /// <summary>
        /// Creates a new <see cref="DiscordChatbot"/> instance.
        /// </summary>
        public DiscordChatbot(
            DiscordSettings settings,
            RequestParser parser,
            ILogger<DiscordChatbot> logger,
            IMediator mediator) :
            base("Discord", settings, parser, logger, mediator) =>
            _client = new DiscordSocketClient();
        /// <inheritdoc/>
        protected override void Initialize()
        {
            _client.Log += HandleClientLog;
            _client.LoggedIn += HandleClientLogin;
            _client.LoggedOut += HandleClientLogout;
            _client.Connected += HandleClientConnect;
            _client.Disconnected += HandleClientDisconnect;
            _client.MessageReceived += HandleClientMessage;
            _client.JoinedGuild += HandleClientGuildJoin;
            _client.GuildScheduledEventCreated += HandleClientScheduledGuildEventCreation;
            _client.InviteCreated += HandleClientInviteCreation;
        }
        /// <inheritdoc/>
        protected override async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await _client.LoginAsync(TokenType.Bot, Settings.Token);
                await _client.StartAsync();
                Log(LogLevel.Information, "The Discord service has been started successfully.");
            } catch (Exception e)
            {
                Log(LogLevel.Information, $"An unexpected error occurred while starting the Discord service. {e.Message}");
            }
        }
        private Task HandleClientInviteCreation(SocketInvite arg) => throw new NotImplementedException();
        private Task HandleClientScheduledGuildEventCreation(SocketGuildEvent arg) => throw new NotImplementedException();
        private Task HandleClientGuildJoin(SocketGuild arg) => throw new NotImplementedException();
        private async Task HandleClientConnect() =>
            Log(LogLevel.Information, "Successfully connected to Discord.");
        private async Task HandleClientDisconnect(Exception arg) =>
            Log(LogLevel.Error, $"Disconnected from Discord. {arg.FlattenMessages(" ")}");
        private async Task HandleClientLogin() =>
            Log(LogLevel.Information, "Logged in to Discord.");
        private async Task HandleClientLogout() =>
            Log(LogLevel.Warning, "Logged out of Discord.");
        private async Task HandleClientLog(LogMessage arg)
        {
            // Determine the event type.
            LogLevel logLevel = arg.Exception is null
                ? LogLevel.Information
                : LogLevel.Error;

            // Log the original message.
            if (!string.IsNullOrWhiteSpace(arg.Message))
                Log(logLevel, arg.Message);

            // Log the exception separately.
            if (arg.Exception is not null)
            {
                try
                {
                    Log(LogLevel.Error, arg.Exception.FlattenMessages(" "));
                } catch (Exception e)
                {
                    Log(LogLevel.Error, $"An unexpected error occurred while recording a log from Discord. {e.Message}");
                }
            }
        }
        private async Task HandleClientMessage(SocketMessage arg) =>
            await Task.CompletedTask;
    }
}
