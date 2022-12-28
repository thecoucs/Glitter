using Freya.Pipeline;

using Mauve.Patterns;
using Mauve.Runtime.Processing;
using Mauve.Runtime.Services;

namespace Freya.Core
{
    internal abstract class BotService : IPipelineService<BotCommand>
    {

        #region Fields

        private CommandPipeline? _commandPipeline;
        private IDependencyCollection? _dependencies;

        #endregion

        #region Properties

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
        }

        #endregion

    }
}
