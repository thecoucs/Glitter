using Freya.Commands;
using Freya.Core;
using Freya.Pipeline;

using Mauve;
using Mauve.Runtime;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Freya.Services
{
    /// <summary>
    /// Represents a service for integrating Freya with a specific provider.
    /// </summary>
    public abstract class Chatbot
    {
        private readonly ILogger _logger;
        protected RequestParser Parser { get; set; }
        protected IMediator Mediator { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Creates a new <see cref="Chatbot"/> instance.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="logger">The logger to be utilized by the service.</param>
        public Chatbot(
            string name,
            RequestParser parser,
            ILogger logger,
            IMediator mediator)
        {
            Name = name;
            _logger = logger;
            Parser = parser;
            Mediator = mediator;
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
        /// <summary>
        /// Starts the <see cref="Chatbot"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> describing the state of the operation.</returns>
        public async Task Start(CancellationToken cancellationToken)
        {
            // Cancel if requested, otherwise create the command pipeline and dependency collection.
            cancellationToken.ThrowIfCancellationRequested();
            Log(LogLevel.Information, "Initializing.");
            Initialize();

            // Cancel if requested, otherwise run the service.
            Log(LogLevel.Information, $"Starting.");
            cancellationToken.ThrowIfCancellationRequested();
            _ = Task.Run(async () =>
            {
                await Run(cancellationToken);

                // Keep the bot alive.
                await Task.Delay(Timeout.Infinite);
            }, cancellationToken);
            await Task.CompletedTask;
        }
        protected virtual void Initialize() { }
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
        protected void Log(LogLevel level, string message) =>
            _logger.Log(level, $"{Name}: {message}");
    }
}
