using System.Collections.Concurrent;
using System.Runtime.Versioning;

using Microsoft.Extensions.Logging;

namespace Freya.Logging
{
    /// <summary>
    /// Represents an implementation of <see cref="ILoggerProvider"/> for providing <see cref="ColoredConsoleLogger"/> instances.
    /// </summary>
    [UnsupportedOSPlatform("browser")]
    [ProviderAlias("ColoredConsole")]
    internal class ColoredConsoleLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ColoredConsoleLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new ColoredConsoleLogger(name));
        /// <inheritdoc/>
        public void Dispose() =>
            _loggers.Clear();
    }
}
