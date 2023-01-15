using System.Reflection;

using Freya.Ai;
using Freya.Commands;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freya
{
    /// <summary>
    /// Represents an interface that exposes methods to configure Freya.
    /// </summary>
    public class SynapseBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly List<Assembly> _chatbotAssemblies;
        /// <summary>
        /// Specifies whether or not a <see cref="Console"/> driven bot is available for testing purposes.
        /// </summary>
        internal bool TestBotEnabled { get; private set; }
        /// <summary>
        /// The prefix utilized to identify commands in a text-only based chat system.
        /// </summary>
        internal string? CommandPrefix { get; private set; }
        /// <summary>
        /// The separator utilized to identify command arguments in a text-only based chat system.
        /// </summary>
        internal string? CommandSeparator { get; private set; }
        internal List<Type> CommandTypes { get; private set; }
        internal SynapseBuilder(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
            _chatbotAssemblies = new List<Assembly>();
            CommandTypes = new List<Type>();
        }
        /// <summary>
        /// Enables a <see cref="Console"/> driven bot for testing purposes.
        /// </summary>
        /// <returns>The current <see cref="SynapseBuilder"/> instance with the testing console enabled.</returns>
        public SynapseBuilder EnableTesting()
        {
            TestBotEnabled = true;
            return this;
        }
        /// <summary>
        /// Sets the prefix utilized to identify commands in a text-only based chat system.
        /// </summary>
        /// <param name="commandPrefix">The prefix utilized to identify commands in a text-only based chat system.</param>
        /// <returns>The current <see cref="SynapseBuilder"/> instance with the command prefix set to the specified value.</returns>
        public SynapseBuilder SetCommandPrefix(string commandPrefix)
        {
            CommandPrefix = commandPrefix;
            return this;
        }
        /// <summary>
        /// Sets the separator utilized to identify command arguments in a text-only based chat system.
        /// </summary>
        /// <param name="commandSeparator">The separator utilized to identify command arguments in a text-only based chat system.</param>
        /// <returns>The current <see cref="SynapseBuilder"/> instance with the command separator set to the specified value.</returns>
        public SynapseBuilder SetCommandSeparator(string commandSeparator)
        {
            CommandSeparator = commandSeparator;
            return this;
        }
        /// <summary>
        /// Adds a chatbot to the DI container.
        /// </summary>
        /// <typeparam name="T">Specifies the type of <see cref="Chatbot"/> to add.</typeparam>
        /// <returns>The current <see cref="ChatbotRegistrar"/> instance with the specified type added as a hosted service.</returns>
        public SynapseBuilder AddChatbot<T>() where T : Chatbot
        {
            _ = _services.AddHostedService<T>();
            var chatbotAssembly = Assembly.GetAssembly(typeof(T));
            if (chatbotAssembly is not null)
                _chatbotAssemblies.Add(chatbotAssembly);

            return this;
        }
        public SynapseBuilder AddCommand<T>() where T : Command
        {
            CommandTypes.Add(typeof(T));
            return this;
        }
        public SynapseBuilder AddSettings<T>(string key) where T : class, new()
        {
            IConfigurationSection? configurationSection = _configuration.GetSection(key);
            T settings = configurationSection is null
                ? new T()
                : configurationSection.Get<T>() ?? new T();

            _ = _services.AddSingleton(settings);
            return this;
        }
        public SynapseBuilder AddServices(Action<IServiceCollection> serviceAction)
        {
            serviceAction?.Invoke(_services);
            return this;
        }
        public SynapseBuilder AddEventHandler<T>() where T : EventHandler
        {
            _ = _services.AddHostedService<T>();
            return this;
        }
        internal IEnumerable<Assembly> GetRegisteredAssemblies() =>
            _chatbotAssemblies.AsReadOnly();
    }
}
