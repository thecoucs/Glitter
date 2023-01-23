using Discord.WebSocket;

using Glitter.Discord.Events;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Glitter.Discord;

/// <summary>
/// Represents a collection of extension methods for adding Discord support to a chatbot application.
/// </summary>
public static class DiscordServiceExtensions
{

    /// <summary>
    /// Adds support for Discord to the DI container.
    /// </summary>
    /// <param name="optionsBuilder">The <see cref="OptionsBuilder<GlitterOptions>"/> to be used for adding Discord to the service contract.</param>
    /// <param name="token">The authentication token that should be used to communicate with Discord.</param>
    /// <returns>The current <see cref="GlitterBuilder"/> instance containing <see cref="DiscordChatbot"/> and its dependencies as a service.</returns>
    public static OptionsBuilder<GlitterOptions> AddDiscord(this OptionsBuilder<GlitterOptions> optionsBuilder, string token) =>
        AddDiscord(optionsBuilder, optionsBuilder =>
            optionsBuilder.Configure(options => options.Token = token)
                          .Validate(options => string.IsNullOrWhiteSpace(options.Token), "A token is required for the Discord chatbot.")
        );
    private static OptionsBuilder<GlitterOptions> AddDiscord(this OptionsBuilder<GlitterOptions> optionsBuilder, Action<OptionsBuilder<DiscordOptions>> configurationAction)
    {
        // Register individual services.
        OptionsBuilder<DiscordOptions> discordOptionsBuilder = optionsBuilder.Services.AddSingleton<DiscordSocketClient>()
            .AddHostedService<LogEventHandler>()
            .AddHostedService<LogEventHandler>()
            .AddHostedService<LoggedInEventHandler>()
            .AddHostedService<LoggedOutEventHandler>()
            .AddHostedService<ConnectedEventHandler>()
            .AddHostedService<DisconnectedEventHandler>()
            .AddHostedService<MessageReceivedEventHandler>()
            .AddOptions<DiscordOptions>()
            .BindConfiguration(configSectionPath: "Discord");

        // Invoke the configuration action.
        configurationAction?.Invoke(discordOptionsBuilder);
        return optionsBuilder;
    }
}