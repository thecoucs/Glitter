using Discord.WebSocket;

using Microsoft.Extensions.Logging;

namespace Glitter.Discord.Events;

/// <summary>
/// Represents an <see cref="EncapsulatedEventHandler"/> for handling the connected event for a <see cref="DiscordSocketClient"/>.
/// </summary>
internal sealed class MessageReceivedEventHandler : EncapsulatedEventHandler
{
    private readonly DiscordSocketClient _client;
    /// <summary>
    /// Creates a new <see cref="MessageReceivedEventHandler"/> instance.
    /// </summary>
    /// <param name="client">The <see cref="DiscordSocketClient"/> to handle connected events for.</param>
    /// <param name="logger">The logger for the <see cref="DiscordChatbot"/>.</param>
    public MessageReceivedEventHandler(DiscordSocketClient client, ILogger<DiscordChatbot> logger) :
        base(logger) =>
        _client = client;
    /// <inheritdoc/>
    protected override void Subscribe() =>
        _client.MessageReceived += HandleMessage;
    /// <inheritdoc/>
    protected override void Unsubscribe() =>
        _client.MessageReceived -= HandleMessage;
    private Task HandleMessage(SocketMessage message)
    {
        Logger.LogDebug($"{message.Author.Username} sent a message: {message.Content}");
        return Task.CompletedTask;
    }
}