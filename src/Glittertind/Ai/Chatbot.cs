using Freya.Commands;
using Freya.Pipeline;

using Mauve.Runtime;

using MediatR;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Glittertind.Ai
{
    /// <summary>
    /// Represents a service for integrating Freya with a specific provider.
    /// </summary>
    public abstract class Chatbot : BackgroundService
    {
        protected ILogger Logger { get; set; }
        protected IMediator Mediator { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Creates a new <see cref="Chatbot"/> instance.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="logger">The logger to be utilized by the service.</param>
        public Chatbot(
            string name,
            IMediator mediator,
            ILogger logger)
        {
            Name = name;
            Mediator = mediator;
            Logger = logger;
        }
        /// <summary>
        /// Configures the <see cref="Chatbot"/>.
        /// </summary>
        /// <param name="services">The service collection to add to.</param>
        /// <param name="pipeline">The pipeline to register middleware with.</param>
        public virtual void Configure(IPipeline<Command> pipeline)
        {
            // Add the basic execution pipeline.
            var executor = new CommandExecutionMiddleware();
            pipeline.Run(executor);
        }
        protected virtual void Initialize() { }
        /// <summary>
        /// Runs the primary work for the service.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        /// <returns>A <see cref="Task"/> describing the state of the operation.</returns>
        protected abstract Task Run(CancellationToken cancellationToken);
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Cancel if requested, otherwise configure the service.
            cancellationToken.ThrowIfCancellationRequested();
            Logger.LogDebug("Configuring.");
            Configure(new CommandPipeline(cancellationToken));

            // Initialize the service.
            cancellationToken.ThrowIfCancellationRequested();
            Logger.LogDebug("Initializing.");
            Initialize();

            // Cancel if requested, otherwise run the service.
            cancellationToken.ThrowIfCancellationRequested();
            Logger.LogDebug($"Starting.");
            await Run(cancellationToken);
        }
    }
}
