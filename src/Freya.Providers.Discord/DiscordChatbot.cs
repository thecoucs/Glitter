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
        private async Task HandleClientMessage(SocketMessage arg) =>
            await Task.CompletedTask;
    }
}
