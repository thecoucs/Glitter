using Discord;
using Discord.WebSocket;

using Mauve.Extensibility;

using Microsoft.Extensions.Logging;

namespace Glitter.Discord.Events;

/// <summary>
/// Represents an <see cref="EncapsulatedEventHandler"/> for handling the Log event for a <see cref="DiscordSocketClient"/>.
/// </summary>
internal sealed class LogEventHandler : EncapsulatedEventHandler
{
    private readonly DiscordSocketClient _client;
    private readonly Dictionary<LogSeverity, LogLevel> _severityToLevelMap;
    /// <summary>
    /// Creates a new <see cref="LogEventHandler"/> instance.
    /// </summary>
    /// <param name="client">The <see cref="DiscordSocketClient"/> to handle log events for.</param>
    /// <param name="logger">The logger for the <see cref="DiscordChatbot"/>.</param>
    public LogEventHandler(DiscordSocketClient client, ILogger<DiscordChatbot> logger) :
        base(logger)
    {
        _client = client;
        _severityToLevelMap = new Dictionary<LogSeverity, LogLevel>
        {
            { LogSeverity.Critical, LogLevel.Critical },
            { LogSeverity.Verbose, LogLevel.Debug },
            { LogSeverity.Error, LogLevel.Error },
            { LogSeverity.Info, LogLevel.Information },
            { LogSeverity.Debug, LogLevel.Trace },
            { LogSeverity.Warning, LogLevel.Warning }
        };
    }
    /// <inheritdoc/>
    protected override void Subscribe() =>
        _client.Log += HandleLogMessage;
    /// <inheritdoc/>
    protected override void Unsubscribe() =>
        _client.Log -= HandleLogMessage;
    private Task HandleLogMessage(LogMessage message)
    {
        // Determine the event type.
        LogLevel logLevel = _severityToLevelMap[message.Severity];

        // Log the original message.
        if (!string.IsNullOrWhiteSpace(message.Message))
            Logger.Log(logLevel, message.Message);

        // Log the exception separately.
        if (message.Exception is not null)
        {
            try
            {
                Logger.Log(LogLevel.Error, message.Exception.FlattenMessages(" "));
            } catch (Exception e)
            {
                Logger.Log(LogLevel.Error, $"An unexpected error occurred while recording a log from Discord. {e.Message}");
                return Task.FromException(e);
            }
        }

        return Task.CompletedTask;
    }
}