using System.Reflection;

using Freya.Core;
using Freya.Services;

using Mauve;

using Microsoft.Extensions.Configuration;

namespace Freya.Runtime
{
    internal class ServiceFactory : AliasedTypeFactory<BotService>
    {

        #region Fields

        private readonly IConfiguration _configuration;
        private readonly CommandFactory _commandFactory;

        #endregion

        #region Constructor

        public ServiceFactory(IConfiguration configuration, CommandFactory commandFactory)
        {
            _configuration = configuration;
            _commandFactory = commandFactory;
        }

        #endregion

        #region Public Methods

        public IEnumerable<BotService> Discover(CancellationToken cancellationToken)
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
                    ? Activator.CreateInstance(type, _commandFactory, cancellationToken)
                    : Activator.CreateInstance(type, settings, _commandFactory, cancellationToken);

                // Validate the instance and register it.
                if (instance is BotService botService)
                    yield return botService;
            }

            // Signal to consumers that we're done iterating.
            yield break;
        }

        #endregion

        #region Private Methods

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

            AliasAttribute? alias = type.GetCustomAttribute<AliasAttribute>();
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

        #endregion

    }
}
