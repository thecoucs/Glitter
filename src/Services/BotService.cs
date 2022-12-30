using Freya.Commands;
using Freya.Core;
using Freya.Pipeline;

using Mauve;
using Mauve.Runtime;
using Mauve.Runtime.Processing;

using Microsoft.Extensions.DependencyInjection;

namespace Freya.Services
{
    /// <summary>
    /// Represents a service for integrating Freya with a specific provider.
    /// </summary>
    internal abstract class BotService
    {
        private readonly string _name;
        private readonly ILogger<LogEntry> _logger;
        private readonly CancellationToken _cancellationToken;
        protected CommandFactory CommandFactory { get; set; }
        /// <summary>
        /// Creates a new <see cref="BotService"/> instance.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="logger">The logger to be utilized by the service.</param>
        /// <param name="commandFactory">The <see cref="Commands.CommandFactory"/> instance the service should use when creating commands.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public BotService(string name, ILogger<LogEntry> logger, CommandFactory commandFactory, CancellationToken cancellationToken)
        {
            _name = name;
            _logger = logger;
            _cancellationToken = cancellationToken;
            CommandFactory = commandFactory;
        }
        /// <summary>
        /// Configures the <see cref="BotService"/>.
        /// </summary>
        /// <param name="services">The service collection to add to.</param>
        /// <param name="pipeline">The pipeline to register middleware with.</param>
        public void Configure(IServiceCollection services, IPipeline<Command> pipeline)
        {
            // Allow the concrete service to configure itself.
            ConfigureService(services, pipeline);

            // Add the basic execution pipeline.
            var executor = new CommandExecutionMiddleware(_cancellationToken);
            pipeline.Run(executor);
        }
        /// <summary>
        /// Starts the <see cref="BotService"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> describing the state of the operation.</returns>
        public async Task Start()
        {
            // Cancel if requested, otherwise create the command pipeline and dependency collection.
            _cancellationToken.ThrowIfCancellationRequested();
            await Log(EventType.Information, $"Starting.");

            // Cancel if requested, otherwise run the service.
            _cancellationToken.ThrowIfCancellationRequested();
            _ = Task.Run(async () =>
            {
                await Run(_cancellationToken);

                // Keep the bot alive.
                await Task.Delay(Timeout.Infinite);
            }, _cancellationToken);
            await Task.CompletedTask;
        }
        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="services">The service collection to add to.</param>
        /// <param name="pipeline">The pipeline to register middleware with.</param>
        protected abstract void ConfigureService(IServiceCollection services, IPipeline<Command> pipeline);
        /// <summary>
        /// Runs the primary work for the service.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        /// <returns>A <see cref="Task"/> describing the state of the operation.</returns>
        protected abstract Task Run(CancellationToken cancellationToken);
        /// <summary>
        /// Records a message with a specified <see cref="EventType"/>.
        /// </summary>
        /// <param name="eventType">The <see cref="EventType"/> associated with the message.</param>
        /// <param name="message">The message to log.</param>
        /// <returns>A <see cref="Task"/> describing the state of the operation.</returns>
        protected async Task Log(EventType eventType, string message)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            await _logger.LogAsync(new LogEntry(eventType, $"{_name}: {message}"));
        }
    }
}
