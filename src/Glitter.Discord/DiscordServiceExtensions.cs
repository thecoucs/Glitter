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
    /// <param name="synapses">The <see cref="GlitterBuilder"/> to be used for adding Discord to the service contract.</param>
    /// <returns>The current <see cref="GlitterBuilder"/> instance containing <see cref="DiscordChatbot"/> and its dependencies as a service.</returns>
    public static GlitterBuilder AddDiscord(this GlitterBuilder specs) =>
        RegisterDiscordServices(specs, settings: null);
    /// <summary>
    /// Adds support for Discord to the DI container.
    /// </summary>
    /// <param name="synapses">The <see cref="GlitterBuilder"/> to be used for adding Discord to the service contract.</param>
    /// <param name="token">The authentication token that should be used to communicate with Discord.</param>
    /// <returns>The current <see cref="GlitterBuilder"/> instance containing <see cref="DiscordChatbot"/> and its dependencies as a service.</returns>
    public static GlitterBuilder AddDiscord(this GlitterBuilder specs, string token)
    {
        // Create a new set of settings for Discord.
        var settings = new DiscordOptions
        {
            Token = token
        };

        // Register services and return the builder.
        return RegisterDiscordServices(specs, settings);
    }
    private static OptionsBuilder<DiscordOptions> RegisterDiscordServices(this GlitterBuilder specs, DiscordOptions? settings)
    {
        // Determine if settings should be loaded or built.
        _ = settings is null
            ? specs.AddSettings<DiscordOptions>("Discord")
            : specs.AddServices(services => services.AddSingleton(settings));

        // Register remaining services and return the builder.
        return specs.AddChatbot<DiscordChatbot>()
                    .AddEventHandler<LogEventHandler>()
                    .AddEventHandler<LogEventHandler>()
                    .AddEventHandler<LoggedInEventHandler>()
                    .AddEventHandler<LoggedOutEventHandler>()
                    .AddEventHandler<ConnectedEventHandler>()
                    .AddEventHandler<DisconnectedEventHandler>()
                    .AddEventHandler<MessageReceivedEventHandler>()
                    .AddServices(services => services.AddSingleton<DiscordSocketClient>());
    }
}