using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freya.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection LoadConfiguration(this IServiceCollection services, out IConfiguration configuration) {
            // Validate the base directory.
            string baseDirectory = AppContext.BaseDirectory;
            if (string.IsNullOrWhiteSpace(baseDirectory))
                throw new InvalidOperationException("The base directory cannot be null or whitespace.");

            // Validate the parent directory.
            DirectoryInfo? parentDirectory = Directory.GetParent(baseDirectory);
            if (parentDirectory is null)
                throw new InvalidOperationException("he parent directory cannot be null.");

            // Build configuration
            Console.WriteLine("Building configuration.");
            configuration = new ConfigurationBuilder()
                .SetBasePath(parentDirectory.FullName)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true)
                .Build();
            
            return services;
        }
    }
}