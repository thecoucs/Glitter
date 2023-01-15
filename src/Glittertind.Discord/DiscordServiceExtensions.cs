using Discord.WebSocket;

using Glittertind.Discord.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Glittertind.Discord
{
    /// <summary>
    /// Represents a collection of extension methods for adding Discord support to a chatbot application.
    /// </summary>
    public static class DiscordServiceExtensions
    {
        /// <summary>
        /// Adds support for Discord to the DI container.
        /// </summary>
        /// <param name="synapses">The <see cref="SynapseBuilder"/> to be used for adding Discord to the service contract.</param>
        /// <returns>The current <see cref="SynapseBuilder"/> instance containing <see cref="DiscordChatbot"/> and its dependencies as a service.</returns>
        public static SynapseBuilder AddDiscord(this SynapseBuilder synapses) =>
            RegisterDiscordServices(synapses, settings: null);
        /// <summary>
        /// Adds support for Discord to the DI container.
        /// </summary>
        /// <param name="synapses">The <see cref="SynapseBuilder"/> to be used for adding Discord to the service contract.</param>
        /// <param name="token">The authentication token that should be used to communicate with Discord.</param>
        /// <returns>The current <see cref="SynapseBuilder"/> instance containing <see cref="DiscordChatbot"/> and its dependencies as a service.</returns>
        public static SynapseBuilder AddDiscord(this SynapseBuilder synapses, string token)
        {
            // Create a new set of settings for Discord.
            var settings = new DiscordSettings
            {
                Token = token
            };

            // Register services and return the builder.
            return RegisterDiscordServices(synapses, settings);
        }
        private static SynapseBuilder RegisterDiscordServices(this SynapseBuilder synapses, DiscordSettings? settings)
        {
            // Determine if settings should be loaded or built.
            _ = settings is null
                ? synapses.AddSettings<DiscordSettings>("Discord")
                : synapses.AddServices(services => services.AddSingleton(settings));

            // Register remaining services and return the builder.
            return synapses.AddChatbot<DiscordChatbot>()
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