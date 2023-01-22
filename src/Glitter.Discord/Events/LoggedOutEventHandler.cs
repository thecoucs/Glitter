using Discord.WebSocket;

using Microsoft.Extensions.Logging;

namespace Glitter.Discord.Events;

/// <summary>
/// Represents an <see cref="EncapsulatedEventHandler"/> for handling the LoggedOut event for a <see cref="DiscordSocketClient"/>.
/// </summary>
internal sealed class LoggedOutEventHandler : EncapsulatedEventHandler
{
    private readonly DiscordSocketClient _client;
    /// <summary>
    /// Creates a new <see cref="LogEventHandler"/> instance.
    /// </summary>
    /// <param name="client">The <see cref="DiscordSocketClient"/> to handle logout events for.</param>
    /// <param name="logger">The logger for the <see cref="DiscordChatbot"/>.</param>
    public LoggedOutEventHandler(DiscordSocketClient client, ILogger<DiscordChatbot> logger) :
        base(logger) =>
        _client = client;
    /// <inheritdoc/>
    protected override void Subscribe() =>
        _client.LoggedOut += HandleLogout;
    /// <inheritdoc/>
    protected override void Unsubscribe() =>
        _client.LoggedOut -= HandleLogout;
    private Task HandleLogout()
    {
        Logger.LogWarning("Logged out of Discord.");
        return Task.CompletedTask;
    }
}