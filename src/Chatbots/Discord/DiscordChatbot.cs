using Discord;
using Discord.WebSocket;
using Freya.Core;
using Mauve;
using Mauve.Extensibility;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Freya.Services.Discord
{
    /// <summary>
    /// Represents a new <see cref="Chatbot"/> for integrating with <see href="https://discordnet.dev/guides/introduction/intro.html">Discord</see>.
    /// </summary>
    [Alias("discord")]
    internal class DiscordChatbot : Chatbot<DiscordSettings>
    {
        private readonly DiscordSocketClient _client;
        /// <summary>
        /// Creates a new <see cref="DiscordChatbot"/> instance.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public DiscordChatbot(
            DiscordSettings settings,
            RequestParser parser,
            ILogger<DiscordChatbot> logger,
            IMediator mediator,
            CancellationToken cancellationToken) :
            base("Discord", settings, parser, logger, mediator, cancellationToken) =>
            _client = new DiscordSocketClient();
        /// <inheritdoc/>
        protected override void Initialize()
        {
            _client.Log += HandleDiscordLog;
            _client.LoggedIn += HandleDiscordLogin;
            _client.LoggedOut += HandleClientLogout;
            _client.Connected += HandleClientConnect;
            _client.Disconnected += HandleClientDisconnect;
            _client.MessageReceived += HandleClientMessage;
            _client.JoinedGuild += HandleGuildJoin;
            _client.GuildScheduledEventCreated += HandleScheduledGuildEventCreation;
            _client.InviteCreated += HandleInviteCreation;
        }
        /// <inheritdoc/>
        protected override async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await _client.LoginAsync(TokenType.Bot, Settings.Token);
                await _client.StartAsync();
                await Log(LogLevel.Information, "The Discord service has been started successfully.");
            } catch (Exception e)
            {
                await Log(LogLevel.Information, $"An unexpected error occurred while starting the Discord service. {e.Message}");
            }
        }
        private Task HandleInviteCreation(SocketInvite arg) => throw new NotImplementedException();
        private Task HandleScheduledGuildEventCreation(SocketGuildEvent arg) => throw new NotImplementedException();
        private Task HandleGuildJoin(SocketGuild arg) => throw new NotImplementedException();
        private async Task HandleClientConnect() =>
            await Log(LogLevel.Information, "Successfully connected to Discord.");
        private async Task HandleClientDisconnect(Exception arg) =>
            await Log(LogLevel.Error, $"Disconnected from Discord. {arg.FlattenMessages(" ")}");
        private async Task HandleDiscordLogin() =>
            await Log(LogLevel.Information, "Logged in to Discord.");
        private async Task HandleClientLogout() =>
            await Log(LogLevel.Warning, "Logged out of Discord.");
        private async Task HandleDiscordLog(LogMessage arg)
        {
            // Determine the event type.
            LogLevel logLevel = arg.Exception is null
                ? LogLevel.Information
                : LogLevel.Error;

            // Log the original message.
            if (!string.IsNullOrWhiteSpace(arg.Message))
                await Log(logLevel, arg.Message);

            // Log the exception separately.
            if (arg.Exception is not null)
            {
                try
                {
                    await Log(LogLevel.Error, arg.Exception.FlattenMessages(" "));
                } catch (Exception e)
                {
                    await Log(LogLevel.Error, $"An unexpected error occurred while recording a log from Discord. {e.Message}");
                }
            }
        }
        private async Task HandleClientMessage(SocketMessage arg) =>
            await Task.CompletedTask;
    }
}
