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
    internal sealed class DiscordChatbot : Chatbot<DiscordSettings>
    {
        private readonly DiscordSocketClient _client;
        /// <summary>
        /// Creates a new <see cref="DiscordChatbot"/> instance.
        /// </summary>
        public DiscordChatbot(
            DiscordSettings settings,
            DiscordSocketClient client,
            RequestParser parser,
            ILogger<DiscordChatbot> logger,
            IMediator mediator) :
            base("Discord", settings, parser, logger, mediator) =>
            _client = client;
        /// <inheritdoc/>
        protected override void Initialize()
        {
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
            }
            catch (Exception e)
            {
                Log(LogLevel.Information, $"An unexpected error occurred while starting the Discord service. {e.Message}");
            }
        }
        private Task HandleClientInviteCreation(SocketInvite arg) => throw new NotImplementedException();
        private Task HandleClientScheduledGuildEventCreation(SocketGuildEvent arg) => throw new NotImplementedException();
        private Task HandleClientGuildJoin(SocketGuild arg) => throw new NotImplementedException();
        private async Task HandleClientConnect()
        {
            Log(LogLevel.Information, "Successfully connected to Discord.");
            await Task.CompletedTask;
        }
        private async Task HandleClientDisconnect(Exception arg)
        {
            Log(LogLevel.Error, $"Disconnected from Discord. {arg.FlattenMessages(" ")}");
            await Task.CompletedTask;
        }
        private async Task HandleClientLogin()
        {
            Log(LogLevel.Information, "Logged in to Discord.");
            await Task.CompletedTask;
        }
        private async Task HandleClientLogout()
        {
            Log(LogLevel.Warning, "Logged out of Discord.");
            await Task.CompletedTask;
        }
        private async Task HandleClientMessage(SocketMessage arg) =>
            await Task.CompletedTask;
    }
}
