using Discord.WebSocket;

using Microsoft.Extensions.Logging;

namespace Glitter.Discord.Events
{
    /// <summary>
    /// Represents an <see cref="EventHandler"/> for handling the LoggedIn event for a <see cref="DiscordSocketClient"/>.
    /// </summary>
    internal sealed class LoggedInEventHandler : EncapsulatedEventHandler
    {
        /// <summary>
        /// Creates a new <see cref="LoggedInEventHandler"/> instance.
        /// </summary>
        /// <param name="client">The <see cref="DiscordSocketClient"/> to handle login events for.</param>
        /// <param name="logger">The logger for the <see cref="DiscordChatbot"/>.</param>
        public LoggedInEventHandler(DiscordSocketClient client, ILogger<DiscordChatbot> logger) :
            base(logger) =>
            client.LoggedIn += HandleLogin;
        private async Task HandleLogin()
        {
            Logger.LogDebug("Logged into Discord.");
            await Task.CompletedTask;
        }
    }
}