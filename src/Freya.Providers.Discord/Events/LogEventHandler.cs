using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Mauve.Extensibility;
using Microsoft.Extensions.Logging;

namespace Freya.Providers.Discord.Events
{
    /// <summary>
    /// Represents an <see cref="IEventHandler"/> for handling the Log event for a <see cref="DiscordSocketClient"/>.
    /// </summary>
    internal sealed class LogEventHandler : IEventHandler
    {
        private readonly ILogger _logger;
        /// <summary>
        /// Creates a new <see cref="LogEventHandler"/> instance.
        /// </summary>
        /// <param name="client">The <see cref="DiscordSocketClient"/> to handle log events for.</param>
        /// <param name="logger">The logger for the <see cref="DiscordChatbot"/>.</param>
        public LogEventHandler(DiscordSocketClient discordClient, ILogger<DiscordChatbot> logger) {
            _logger = logger;
            discordClient.Log += HandleLogMessage;
            _logger.LogInformation("Message handler successfully created.");
        }
        private async Task HandleLogMessage(LogMessage message)
        {
            // Determine the event type.
            LogLevel logLevel = message.Exception is null
                ? LogLevel.Information
                : LogLevel.Error;

            // Log the original message.
            if (!string.IsNullOrWhiteSpace(message.Message))
                _logger.Log(logLevel, message.Message);

            // Log the exception separately.
            if (message.Exception is not null)
            {
                try
                {
                    _logger.Log(LogLevel.Error, message.Exception.FlattenMessages(" "));
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Error, $"An unexpected error occurred while recording a log from Discord. {e.Message}");
                    await Task.FromException(e);
                }
            }

            await Task.CompletedTask;
        }
    }
}