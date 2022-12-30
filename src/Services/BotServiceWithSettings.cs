using Freya.Core;
using Freya.Runtime;

using Mauve.Runtime;

namespace Freya.Services
{
    /// <summary>
    /// Represents a <see cref="BotService"/> with settings.
    /// </summary>
    /// <typeparam name="TSettings">Specifies the type used for settings.</typeparam>
    internal abstract class BotService<TSettings> : BotService
    {
        /// <summary>
        /// The settings for the service.
        /// </summary>
        protected TSettings Settings { get; set; }
        /// <summary>
        /// Creates a new <see cref="BotService{TSettings}"/> instance.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="settings">The settings for the service.</param>
        /// <param name="logger">The logger to be utilized by the service.</param>
        /// <param name="commandFactory">The <see cref="Freya.Runtime.CommandFactory"/> instance the service should use when creating commands.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public BotService(string name, TSettings settings, ILogger<LogEntry> logger, CommandFactory commandFactory, CancellationToken cancellationToken) :
            base(name, logger, commandFactory, cancellationToken) =>
            Settings = settings;
    }
}
