using Freya.Commands;
using Mauve.Extensibility;
using Mauve.Patterns;
using Mauve.Runtime.Processing;

namespace Freya
{
    /// <summary>
    /// Represents an implementation of <see cref="IPipeline{T}"/> that executes <see cref="BotCommand"/> instances.
    /// </summary>
    internal class CommandPipeline : IPipeline<BotCommand>
    {

        #region Fields

        private bool _pipelineComplete;
        private readonly CancellationToken _cancellationToken;
        private readonly List<IMiddleware<BotCommand>> _middlewares;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="CommandPipeline"/>.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public CommandPipeline(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _middlewares = new List<IMiddleware<BotCommand>>();
        }

        #endregion

        #region Public Methods

        public async Task Execute(BotCommand input)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            foreach (IMiddleware<BotCommand> middleware in _middlewares)
                await middleware.Invoke(input, _middlewares.NextOrDefault(middleware), _cancellationToken);

            await Task.CompletedTask;
        }
        public void Run(IMiddleware<BotCommand> middleware)
        {
            Validate();
            _middlewares.Add(middleware);
            _pipelineComplete = true;
        }
        public IPipeline<BotCommand> Use(IMiddleware<BotCommand> middleware)
        {
            Validate();
            _middlewares.Add(middleware);
            return this;
        }

        #endregion

        #region Private Methods

        private void Validate()
        {
            if (_pipelineComplete)
                throw new InvalidOperationException("The pipeline has already been completed.");
        }

        #endregion

    }
}
