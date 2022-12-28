using Freya.Pipeline;

using Mauve;
using Mauve.Patterns;
using Mauve.Runtime;
using Mauve.Runtime.Processing;
using Mauve.Runtime.Services;

namespace Freya.Core
{
    internal abstract class BotService : IPipelineService<BotCommand>
    {

        #region Fields

        private CommandPipeline? _commandPipeline;
        private IDependencyCollection? _dependencies;
        private readonly ILogger<LogEntry> _logger;
        private readonly CancellationToken _cancellationToken;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public BotService(ILogger<LogEntry> logger, CancellationToken cancellationToken)
        {
            _logger = logger;
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
            _commandPipeline.Run(new CommandExecutionMiddleware());

            // Cancel if requested, otherwise run the service.
            _cancellationToken.ThrowIfCancellationRequested();
            await Run(_cancellationToken);

            // Keep the bot alive.
            await Task.Delay(Timeout.Infinite);
        }

        #endregion

        #region Protected Methods

        protected abstract Task Run(CancellationToken cancellationToken);
        protected void Log(EventType eventType, string message)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            _logger.Log(new LogEntry(eventType, message));
        }

        #endregion

    }
}
