using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Freya.Providers.Discord.Events
{
    /// <summary>
    /// Represents an <see cref="IEventHandler"/> for handling the LoggedOut event for a <see cref="DiscordSocketClient"/>.
    /// </summary>
    internal sealed class LoggedOutEventHandler : IEventHandler
    {
        private readonly ILogger _logger;
        /// <summary>
        /// Creates a new <see cref="LogEventHandler"/> instance.
        /// </summary>
        /// <param name="client">The <see cref="DiscordSocketClient"/> to handle logout events for.</param>
        /// <param name="logger">The logger for the <see cref="DiscordChatbot"/>.</param>
        public LoggedOutEventHandler(DiscordSocketClient client, ILogger<DiscordChatbot> logger)
        {
            _logger = logger;
            client.LoggedOut += HandleLogout;
        }
        private async Task HandleLogout()
        {
            _logger.LogWarning("Logged out of Discord.");
            await Task.CompletedTask;
        }
    }
}