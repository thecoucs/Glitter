using Discord.WebSocket;

using Glittertind.Discord.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Glittertind.Discord
{
    public static class DiscordServiceExtensions
    {
        /// <summary>
        /// Adds the <see cref="DiscordChatbot"/> to the DI container.
        /// </summary>
        /// <param name="services">The service contract for adding services to the DI container.</param>
        /// <returns>The current <see cref="IServiceCollection"/> instance containing <see cref="DiscordChatbot"/> as a singleton service.</returns>
        public static SynapseBuilder AddDiscord(this SynapseBuilder registrar) =>
            registrar.AddChatbot<DiscordChatbot>()
                     .AddSettings<DiscordSettings>("discord")
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