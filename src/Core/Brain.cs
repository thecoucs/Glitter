using System.Reflection;

using Freya.Runtime;
using Freya.Services;

using Mauve;
using Mauve.Extensibility;

using Microsoft.Extensions.Configuration;

namespace Freya.Core
{
    internal class Brain
    {

        #region Fields

        private readonly List<BotService> _services;
        private readonly CommandFactory _commandFactory;
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Constructor

        public Brain()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _commandFactory = new CommandFactory();
            _services = new List<BotService>();
        }

        #endregion

        #region Public Methods

        public void Cancel() =>
            _cancellationTokenSource.Cancel();
        public void Configure(IConfiguration configuration)
        {
            // Cancel if requested, otherwise load settings.
            _cancellationToken.ThrowIfCancellationRequested();
            RegisterServices(configuration);
        }
        public async Task Start()
        {
            // Cancel if requested, otherwise start each service.
            _cancellationToken.ThrowIfCancellationRequested();
            foreach (BotService service in _services)
            {
                // Cancel if requested, otherwise start the service.
                _cancellationToken.ThrowIfCancellationRequested();
                await service.Start();
            }

            await Task.Delay(Timeout.Infinite);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines if the specified type is a concrete implementation of <see cref="BotService"/>.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        private bool IsConcreteBotService(Type type) =>
            type.DerivesFrom<BotService>() &&
            type.IsAbstract == false;
        // TODO: Add logging. Refactor. Reduce nesting.
        private void RegisterServices(IConfiguration configuration)
        {
            // Get the entry assembly, if we can't find it, there's no work.
            var assembly = Assembly.GetEntryAssembly();
            if (assembly is null)
                return;

            // Get any defined service types, if we have none there's no work.
            IEnumerable<Type> serviceTypes = assembly.GetTypes().Where(IsConcreteBotService);
            if (serviceTypes?.Any() != true)
                return;

            // Register each service type.
            foreach (Type serviceType in serviceTypes)
            {
                // Currently, an alias is required for settings.
                object? settings = null;
                AliasAttribute? alias = serviceType.GetCustomAttribute<AliasAttribute>();
                if (alias is not null)
                {
                    // If there's more than one alias, we don't know which one to use.
                    if (alias.Values.Count > 1)
                        continue;

                    // Get the configuration section.
                    string? key = alias.Values.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        // Attempt to get a config section for the alias.
                        IConfigurationSection configurationSection = configuration.GetSection(key);
                        if (configurationSection is null)
                            continue;

                        // Bots with settings specify a type.
                        Type[]? genericArguments = serviceType?.BaseType?.GetGenericArguments();
                        if (genericArguments?.Length != 1)
                            continue;

                        // Get the setting type, if one is not present, there's no work.
                        Type? settingType = genericArguments.FirstOrDefault();
                        if (settingType is null)
                            continue;

                        // Attempt to parse the settings.
                        settings = configurationSection.Get(settingType);
                    }
                }

                // Validate the service type to satisfy warnings.
                if (serviceType is null)
                    continue;

                // Create an instance of the service.
                object? instance = settings is null
                    ? Activator.CreateInstance(serviceType, _commandFactory, _cancellationToken)
                    : Activator.CreateInstance(serviceType, settings, _commandFactory, _cancellationToken);

                // Validate the instance and register it.
                if (instance is BotService botService)
                    _services.Add(botService);
            }
        }

        #endregion

    }
}
