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

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public BotService(ILogger<LogEntry> logger) =>
            _logger = logger;

        #endregion

        #region Public Methods

        public abstract void Configure(IDependencyCollection dependencies, IPipeline<BotCommand> pipeline);
        public async Task Start()
        {

            // Create the initial dependency collection.
            _dependencies = new DependencyCollection();

            // Create the initial command pipeline.
            _commandPipeline = new CommandPipeline();

            // Allow implementing services to configure their dependencies and custom pipelines.
            Configure(_dependencies, _commandPipeline);

            // Add the basic execution pipeline.
            _commandPipeline.Run(new CommandExecutionMiddleware());

            // Run the service.
            await Run();
        }

        #endregion

        #region Protected Methods

        protected abstract Task Run();
        protected void Log(EventType eventType, string message) =>
            _logger.Log(new LogEntry(eventType, message));

        #endregion

    }
}
