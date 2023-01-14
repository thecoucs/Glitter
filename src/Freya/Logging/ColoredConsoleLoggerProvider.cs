using System.Collections.Concurrent;
using System.Runtime.Versioning;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Freya.Logging
{
    /// <summary>
    /// Represents an implementation of <see cref="ILoggerProvider"/> for providing <see cref="ColoredConsoleLogger"/> instances.
    /// </summary>
    [UnsupportedOSPlatform("browser")]
    [ProviderAlias("ColoredConsole")]
    internal class ColoredConsoleLoggerProvider : ILoggerProvider
    {
        private ColoredConsoleLoggerConfiguration _currentConfiguration;
        private readonly IDisposable? _onChangeToken;
        private readonly ConcurrentDictionary<string, ColoredConsoleLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// Creates a new <see cref="ColoredConsoleLoggerProvider"/> instance.
        /// </summary>
        /// <param name="configurationMonitor">The monitor for the configuration of the logger.</param>
        public ColoredConsoleLoggerProvider(IOptionsMonitor<ColoredConsoleLoggerConfiguration> configurationMonitor)
        {
            _currentConfiguration = configurationMonitor.CurrentValue;
            _onChangeToken = configurationMonitor.OnChange(updatedConfig => _currentConfiguration = updatedConfig);
        }
        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new ColoredConsoleLogger(name, GetCurrentConfiguration));
        /// <inheritdoc/>
        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken?.Dispose();
        }
        private ColoredConsoleLoggerConfiguration GetCurrentConfiguration() =>
            _currentConfiguration;
    }
}
