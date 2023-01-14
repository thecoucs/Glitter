using Discord.WebSocket;
using Freya.Providers.Discord.Events;
using Freya.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freya.Providers.Discord
{
    public static class DiscordServiceExtensions
    {
        /// <summary>
        /// Adds the <see cref="DiscordChatbot"/> to the DI container.
        /// </summary>
        /// <param name="services">The service contract for adding services to the DI container.</param>
        /// <returns>The current <see cref="IServiceCollection"/> instance containing <see cref="DiscordChatbot"/> as a singleton service.</returns>
        public static IServiceCollection AddDiscord(this IServiceCollection services, IConfiguration configuration)
        {
            // Attempt to get a config section for the discord settings.
            IConfigurationSection configurationSection = configuration.GetSection("discord");
            if (configurationSection is null)
                throw new InvalidOperationException("Unable to identify a configuration section for Discord.");

            // Attempt to parse the settings.
            DiscordSettings? settings = configurationSection.Get<DiscordSettings>();
            if (settings is null)
                throw new InvalidOperationException("Unable to load configuration for Discord.");

            // Register the Discord bot and its settings.
            return services.AddSingleton(new DiscordSocketClient())
                           .AddSingleton<Chatbot, DiscordChatbot>()
                           .AddTransient<IEventHandler, LogMessageHandler>()
                           .AddSingleton(settings);
        }
    }
}