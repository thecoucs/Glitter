using Discord;
using Discord.WebSocket;

using Freya.Core;

using Mauve;
using Mauve.Extensibility;
using Mauve.Runtime;

using MediatR;

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
        public DiscordChatbot(DiscordSettings settings, ILogger<LogEntry> logger, IMediator mediator, CancellationToken cancellationToken) :
            base("Discord", settings, logger, mediator, cancellationToken) =>
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
                await Log(EventType.Success, "The Discord service has been started successfully.");
            } catch (Exception e)
            {
                await Log(EventType.Exception, $"An unexpected error occurred while starting the Discord service. {e.Message}");
            }
        }
        private Task HandleInviteCreation(SocketInvite arg) => throw new NotImplementedException();
        private Task HandleScheduledGuildEventCreation(SocketGuildEvent arg) => throw new NotImplementedException();
        private Task HandleGuildJoin(SocketGuild arg) => throw new NotImplementedException();
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
    }
}
