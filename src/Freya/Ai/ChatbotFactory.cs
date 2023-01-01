using System.Reflection;

using Freya.Commands;
using Freya.Core;

using Mauve;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freya.Services
{
    /// <summary>
    /// Represents a factory responsible for creating <see cref="Chatbot"/> instances.
    /// </summary>
    internal class ChatbotFactory : AliasedTypeFactory<Chatbot>
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// Creates a new <see cref="ChatbotFactory"/> instance.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="commandFactory">The factory services should use when creating <see cref="Command"/> instances.</param>
        public ChatbotFactory(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// Discovers all concrete <see cref="Chatbot"/> implementations in the current <see cref="Assembly"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to be provided to the <see cref="Chatbot"/> upon creation.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all concrete <see cref="Chatbot"/> implementations in the current <see cref="Assembly"/>.</returns>
        public IEnumerable<Chatbot> Discover(CancellationToken cancellationToken)
        {
            // Get any types considered valid for the factory.
            IEnumerable<Type>? types = GetQualifiedTypes();
            if (types is null)
                yield break;

            // Attempt to create an instance of each type.
            foreach (Type type in types)
            {
                // Currently, an alias is required for settings.
                if (!TryGetSettings(type, out object? settings))
                    continue;

                // Validate the type.
                if (type is null)
                    continue;

                // Create an instance of the service.
                object? instance = settings is null
                    ? ActivatorUtilities.CreateInstance(_serviceProvider, type, cancellationToken)
                    : ActivatorUtilities.CreateInstance(_serviceProvider, type, settings, cancellationToken);

                // Validate the instance and register it.
                if (instance is Chatbot botService)
                    yield return botService;
            }

            // Signal to consumers that we're done iterating.
            yield break;
        }
        /// <summary>
        /// Attempts to load settings for the specified type.
        /// </summary>
        /// <param name="type">The type to load settings for.</param>
        /// <param name="settings">The resulting settings loaded from the application configuration.</param>
        /// <returns><see langword="true"/> if settings are not applicable or if they were successfully loaded, otherwise <see langword="false"/>.</returns>
        private bool TryGetSettings(Type type, out object? settings)
        {
            // Set settings to null by default and validate the input.
            settings = null;
            if (type is null)
                return false;

            // Bots with settings specify a type.
            Type[]? genericArguments = type?.BaseType?.GetGenericArguments();
            if (genericArguments?.Length == 0)
                return true;
            else if (genericArguments?.Length != 1)
                return false;

            AliasAttribute? alias = type?.GetCustomAttribute<AliasAttribute>();
            if (alias is not null)
            {
                // If there's more than one alias, we don't know which one to use.
                if (alias.Values.Count > 1)
                    return false;

                // Get the configuration section.
                string? key = alias.Values.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(key))
                {
                    // Attempt to get a config section for the alias.
                    IConfigurationSection configurationSection = _configuration.GetSection(key);
                    if (configurationSection is null)
                        return false;

                    // Get the setting type, if one is not present, there's no work.
                    Type? settingType = genericArguments.FirstOrDefault();
                    if (settingType is null)
                        return false;

                    // Attempt to parse the settings.
                    settings = configurationSection.Get(settingType);
                }
            }

            return true;
        }
    }
}
