using System.Reflection;

using Freya.Core;

using Glittertind.Ai;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Glittertind.Extensibility
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseFreya(this IServiceCollection services, Action<SynapseBuilder> configurationAction)
        {
            // Load the configuration.
            LoadConfiguration(out IConfiguration configuration);
            _ = services.AddSingleton(configuration);

            // Allow consumers to configure Freya.
            var synapseBuilder = new SynapseBuilder(services, configuration);
            configurationAction?.Invoke(synapseBuilder);

            // Add the request parser.
            string commandToken = synapseBuilder.CommandPrefix ?? "!";
            string commandSeparator = synapseBuilder.CommandSeparator ?? ",";
            _ = services.AddSingleton(new RequestParser(commandToken, commandSeparator));

            // Add the test bot if it's been enabled.
            if (synapseBuilder.TestBotEnabled)
                _ = services.AddHostedService<ConsoleChatbot>();

            // Add MediatR.
            IEnumerable<Assembly> assemblies = synapseBuilder.GetRegisteredAssemblies().Append(Assembly.GetExecutingAssembly());
            return services.AddMediatR(assemblies: assemblies.ToArray());
        }
        private static void LoadConfiguration(out IConfiguration configuration)
        {
            // Validate the base directory.
            string baseDirectory = AppContext.BaseDirectory;
            if (string.IsNullOrWhiteSpace(baseDirectory))
                throw new InvalidOperationException("The base directory cannot be null or whitespace.");

            // Validate the parent directory.
            DirectoryInfo? parentDirectory = Directory.GetParent(baseDirectory);
            if (parentDirectory is null)
                throw new InvalidOperationException("The parent directory cannot be null.");

            // Build configuration.
            configuration = new ConfigurationBuilder()
                .SetBasePath(parentDirectory.FullName)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true)
                .Build();
        }
    }
}
