using Discord.WebSocket;

using Mauve.Extensibility;

using Microsoft.Extensions.Logging;

namespace Glitter.Discord.Events;

/// <summary>
/// Represents an <see cref="EncapsulatedEventHandler"/> for handling the disconnected event for a <see cref="DiscordSocketClient"/>.
/// </summary>
internal sealed class DisconnectedEventHandler : EncapsulatedEventHandler
{
    private readonly DiscordSocketClient _client;
    /// <summary>
    /// Creates a new <see cref="ConnectedEventHandler"/> instance.
    /// </summary>
    /// <param name="client">The <see cref="DiscordSocketClient"/> to handle disconnected events for.</param>
    /// <param name="logger">The logger for the <see cref="DiscordChatbot"/>.</param>
    public DisconnectedEventHandler(DiscordSocketClient client, ILogger<DiscordChatbot> logger) :
        base(logger) =>
        _client = client;
    /// <inheritdoc/>
    protected override void Subscribe() =>
        _client.Disconnected += HandleDisconnect;
    /// <inheritdoc/>
    protected override void Unsubscribe() =>
        _client.Disconnected -= HandleDisconnect;
    private Task HandleDisconnect(Exception arg)
    {
        Logger.LogError($"Disconnected from Discord. {arg.FlattenMessages(" ")}");
        return Task.CompletedTask;
    }
}