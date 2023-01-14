using Freya.Services;

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
        public static IServiceCollection AddDiscord(this IServiceCollection services) =>
            services.AddSingleton<Chatbot, DiscordChatbot>();
    }
}