using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freya.Ai
{
    /// <summary>
    /// Represents a simple registrar for adding chatbots to the DI container.
    /// </summary>
    public class ChatbotRegistrar
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly List<Assembly> _chatbotAssemblies;
        /// <summary>
        /// Creates a new <see cref="ChatbotRegistrar"/> instance.
        /// </summary>
        /// <param name="services">The contract for registering services with the DI container.</param>
        internal ChatbotRegistrar(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
            _chatbotAssemblies = new List<Assembly>();
        }
        internal IEnumerable<Assembly> GetRegisteredAssemblies() =>
            _chatbotAssemblies.AsReadOnly();
        /// <summary>
        /// Adds a chatbot to the DI container.
        /// </summary>
        /// <typeparam name="T">Specifies the type of <see cref="Chatbot"/> to add.</typeparam>
        /// <returns>The current <see cref="ChatbotRegistrar"/> instance with the specified type added as a hosted service.</returns>
        public ChatbotRegistrar AddChatbot<T>() where T : Chatbot
        {
            _ = _services.AddHostedService<T>();
            var chatbotAssembly = Assembly.GetAssembly(typeof(T));
            if (chatbotAssembly is not null)
                _chatbotAssemblies.Add(chatbotAssembly);

            return this;
        }
        public ChatbotRegistrar AddSettings<T>(string key) where T : class, new()
        {
            IConfigurationSection? configurationSection = _configuration.GetSection(key);
            T settings = configurationSection is null
                ? new T()
                : configurationSection.Get<T>() ?? new T();

            _ = _services.AddSingleton(settings);
            return this;
        }
        public ChatbotRegistrar AddServices(Action<IServiceCollection> serviceAction)
        {
            serviceAction?.Invoke(_services);
            return this;
        }
        public ChatbotRegistrar AddEventHandler<T>() where T : EventHandler
        {
            _ = _services.AddHostedService<T>();
            return this;
        }
    }
}
