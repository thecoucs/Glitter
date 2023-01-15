using Discord.WebSocket;

using Microsoft.Extensions.Logging;

namespace Glittertind.Discord.Events
{
    /// <summary>
    /// Represents an <see cref="EventHandler"/> for handling the connected event for a <see cref="DiscordSocketClient"/>.
    /// </summary>
    internal sealed class ConnectedEventHandler : EncapsulatedEventHandler
    {
        /// <summary>
        /// Creates a new <see cref="ConnectedEventHandler"/> instance.
        /// </summary>
        /// <param name="client">The <see cref="DiscordSocketClient"/> to handle connected events for.</param>
        /// <param name="logger">The logger for the <see cref="DiscordChatbot"/>.</param>
        public ConnectedEventHandler(DiscordSocketClient client, ILogger<DiscordChatbot> logger) :
            base(logger) =>
            client.Connected += HandleConnected;
        private async Task HandleConnected()
        {
            Logger.LogInformation("Successfully connected to Discord.");
            await Task.CompletedTask;
        }
    }
}