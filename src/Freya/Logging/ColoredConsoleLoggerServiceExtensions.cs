using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Freya.Logging
{
    internal static class ColoredConsoleLoggerServiceExtensions
    {
        /// <summary>
        /// Adds a colored console to the logging service.
        /// </summary>
        /// <param name="loggingBuilder">The <see cref="ILoggingBuilder"/> to add the <see cref="ColoredConsoleLogger"/> to.</param>
        /// <returns>The current <see cref="ILoggingBuilder"/> instance with the <see cref="ColoredConsoleLogger"/> added.</returns>
        public static ILoggingBuilder AddColoredConsole(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddConfiguration();
            _ = loggingBuilder.Services.AddSingleton<ILoggerProvider, ColoredConsoleLoggerProvider>();
            LoggerProviderOptions.RegisterProviderOptions<ColoredConsoleLoggerConfiguration, ColoredConsoleLoggerProvider>(loggingBuilder.Services);
            return loggingBuilder;
        }
    }
}
