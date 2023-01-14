using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Freya.Providers.Discord.Events
{
    /// <summary>
    /// Represents an <see cref="IEventHandler"/> for handling the LoggedIn event for a <see cref="DiscordSocketClient"/>.
    /// </summary>
    internal class LoggedInEventHandler : IEventHandler
    {
        private readonly ILogger _logger;
        /// <summary>
        /// Creates a new <see cref="LoggedInEventHandler"/> instance.
        /// </summary>
        /// <param name="client">The <see cref="DiscordSocketClient"/> to handle login events for.</param>
        /// <param name="logger">The logger for the <see cref="DiscordChatbot"/>.</param>
        public LoggedInEventHandler(DiscordSocketClient client, ILogger<DiscordChatbot> logger) {
            _logger = logger;
            client.LoggedIn += HandleLogin;
        }
        private async Task HandleLogin()
        {
            _logger.LogInformation("Logged in to Discord.");
            await Task.CompletedTask;
        }
    }
}