using Discord.WebSocket;

using Microsoft.Extensions.Logging;

namespace Freya.Providers.Discord.Events
{
    /// <summary>
    /// Represents an <see cref="EventHandler"/> for handling the connected event for a <see cref="DiscordSocketClient"/>.
    /// </summary>
    internal sealed class MessageReceivedEventHandler : EventHandler
    {
        /// <summary>
        /// Creates a new <see cref="MessageReceivedEventHandler"/> instance.
        /// </summary>
        /// <param name="client">The <see cref="DiscordSocketClient"/> to handle connected events for.</param>
        /// <param name="logger">The logger for the <see cref="DiscordChatbot"/>.</param>
        public MessageReceivedEventHandler(DiscordSocketClient client, ILogger<DiscordChatbot> logger) :
            base(logger) => client.MessageReceived += HandleMessage;
        private async Task HandleMessage(SocketMessage message)
        {
            Logger.LogDebug($"{message.Author.Username} sent a message: {message.Content}");
            await Task.CompletedTask;
        }
    }
}