using Discord.WebSocket;

using Glitter.Discord.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Glitter.Discord
{
    /// <summary>
    /// Represents a collection of extension methods for adding Discord support to a chatbot application.
    /// </summary>
    public static class DiscordServiceExtensions
    {
        /// <summary>
        /// Adds support for Discord to the DI container.
        /// </summary>
        /// <param name="synapses">The <see cref="GlitterConfigurationBuilder"/> to be used for adding Discord to the service contract.</param>
        /// <returns>The current <see cref="GlitterConfigurationBuilder"/> instance containing <see cref="DiscordChatbot"/> and its dependencies as a service.</returns>
        public static GlitterConfigurationBuilder AddDiscord(this GlitterConfigurationBuilder config) =>
            RegisterDiscordServices(config, settings: null);
        /// <summary>
        /// Adds support for Discord to the DI container.
        /// </summary>
        /// <param name="synapses">The <see cref="GlitterConfigurationBuilder"/> to be used for adding Discord to the service contract.</param>
        /// <param name="token">The authentication token that should be used to communicate with Discord.</param>
        /// <returns>The current <see cref="GlitterConfigurationBuilder"/> instance containing <see cref="DiscordChatbot"/> and its dependencies as a service.</returns>
        public static GlitterConfigurationBuilder AddDiscord(this GlitterConfigurationBuilder config, string token)
        {
            // Create a new set of settings for Discord.
            var settings = new DiscordSettings
            {
                Token = token
            };

            // Register services and return the builder.
            return RegisterDiscordServices(config, settings);
        }
        private static GlitterConfigurationBuilder RegisterDiscordServices(this GlitterConfigurationBuilder config, DiscordSettings? settings)
        {
            // Determine if settings should be loaded or built.
            _ = settings is null
                ? config.AddSettings<DiscordSettings>("Discord")
                : config.AddServices(services => services.AddSingleton(settings));

            // Register remaining services and return the builder.
            return config.AddChatbot<DiscordChatbot>()
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
}