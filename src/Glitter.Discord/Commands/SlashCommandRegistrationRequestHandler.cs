using Discord;
using Discord.Net;
using Discord.WebSocket;

using Glitter.Commands;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Glitter.Discord.Commands;

/// <summary>
/// Represents a handler for <see cref="SlashCommandRegistrationRequest"/>s.
/// </summary>
internal class SlashCommandRegistrationRequestHandler : IRequestHandler<SlashCommandRegistrationRequest>
{
    private bool _workComplete;
    private Command? _command;
    private readonly ILogger _logger;
    private readonly DiscordSocketClient _client;
    public SlashCommandRegistrationRequestHandler(DiscordSocketClient client, ILogger<DiscordChatbot> logger)
    {
        _client = client;
        _logger = logger;
        _workComplete = false;
    }
    public async Task<Unit> Handle(SlashCommandRegistrationRequest request, CancellationToken cancellationToken)
    {
        _command = request.Command;
        _client.Ready += DiscordClientReady;
        while (!_workComplete)
            await Task.Delay(TimeSpan.FromSeconds(1));

        return await Task.FromResult(Unit.Value);
    }
    private async Task DiscordClientReady()
    {
        // Validate the command.
        if (_command is null)
        {
            _logger.LogError($"Cannot register an unspecified command.");
            return;
        }

        try
        {
            // Build the command properties.
            SlashCommandProperties commandProperties = new SlashCommandBuilder()
                .WithName(_command.Key)
                .WithDescription(_command.Description)
                .Build();

            // Register the command globally.
            // TODO: Add support for guild focused registration.
            _ = await _client.CreateGlobalApplicationCommandAsync(commandProperties);
        }
        catch (HttpException e)
        {
            LogHttpException(e);
        }
        finally
        {
            _workComplete = true;
        }
    }
    private void LogHttpException(HttpException e) => _logger.LogTrace($@"Unable to register command.
    Key: {_command?.Key ?? "unspecified"}
    Description: {_command?.Description ?? "unspecified"}
    Code: {e.DiscordCode}
    Reason: {e.Reason}
    Message: {e.Message}
    Errors: {string.Join($"{Environment.NewLine}        ", e.Errors.Select(s => s.Errors.Select(error => $"{error.Code}: {error.Message}")))}");
}
