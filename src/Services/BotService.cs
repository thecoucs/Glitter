using Freya.Commands;
using Freya.Core;
using Freya.Pipeline;
using Freya.Runtime;

using Mauve;
using Mauve.Runtime;
using Mauve.Runtime.Processing;

using Microsoft.Extensions.DependencyInjection;

namespace Freya.Services
{
    internal abstract class BotService
    {

        #region Fields

        private readonly string _name;
        private readonly ILogger<LogEntry> _logger;
        private readonly CancellationToken _cancellationToken;

        #endregion

        #region Properties

        protected CommandFactory CommandFactory { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public BotService(string name, ILogger<LogEntry> logger, CommandFactory commandFactory, CancellationToken cancellationToken)
        {
            _name = name;
            _logger = logger;
            _cancellationToken = cancellationToken;
            CommandFactory = commandFactory;
        }

        #endregion

        #region Public Methods

        public void Configure(IServiceCollection services, IPipeline<Command> pipeline)
        {
            // Allow the concrete service to configure itself.
            ConfigureService(services, pipeline);

            // Add the basic execution pipeline.
            var executor = new CommandExecutionMiddleware(_cancellationToken);
            pipeline.Run(executor);
        }
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

        #endregion

        #region Protected Methods

        protected abstract void ConfigureService(IServiceCollection services, IPipeline<Command> pipeline);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        /// <returns>A <see cref="Task"/> describing the state of execution.</returns>
        protected abstract Task Run(CancellationToken cancellationToken);
        protected async Task Log(EventType eventType, string message)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            await _logger.LogAsync(new LogEntry(eventType, $"{_name}: {message}"));
        }

        #endregion

    }
}
