using Freya.Commands;
using Freya.Pipeline;
using Freya.Runtime;

using Mauve;
using Mauve.Patterns;
using Mauve.Runtime;
using Mauve.Runtime.Processing;
using Mauve.Runtime.Services;

namespace Freya.Services
{
    internal abstract class BotService : IPipelineService<BotCommand>
    {

        #region Fields

        private CommandPipeline? _commandPipeline;
        private IDependencyCollection? _dependencies;
        private readonly ILogger<LogEntry> _logger;
        private readonly CommandFactory _commandFactory;
        private readonly CancellationToken _cancellationToken;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public BotService(ILogger<LogEntry> logger, CommandFactory commandFactory, CancellationToken cancellationToken)
        {
            _logger = logger;
            _commandFactory = commandFactory;
            _cancellationToken = cancellationToken;
        }

        #endregion

        #region Public Methods

        public abstract void Configure(IDependencyCollection dependencies, IPipeline<BotCommand> pipeline);
        public async Task Start()
        {
            // Cancel if requested, otherwise create the command pipeline and dependency collection.
            _cancellationToken.ThrowIfCancellationRequested();
            _commandPipeline = new CommandPipeline(_cancellationToken);
            _dependencies = new DependencyCollection();

            // Allow implementing services to configure their dependencies and custom pipelines.
            Configure(_dependencies, _commandPipeline);

            // Add the basic execution pipeline.
            var executor = new CommandExecutionMiddleware(_cancellationToken);
            _commandPipeline.Run(executor);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        /// <returns>A <see cref="Task"/> describing the state of execution.</returns>
        protected abstract Task Run(CancellationToken cancellationToken);
        protected async Task Log(EventType eventType, string message)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            await _logger.LogAsync(new LogEntry(eventType, message), _cancellationToken);
        }

        #endregion

    }
}
