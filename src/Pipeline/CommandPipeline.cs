using Freya.Commands;
using Mauve.Extensibility;
using Mauve.Patterns;
using Mauve.Runtime.Processing;

namespace Freya.Pipeline
{
    /// <summary>
    /// Represents an implementation of <see cref="IPipeline{T}"/> that executes <see cref="Command"/> instances.
    /// </summary>
    internal class CommandPipeline : IPipeline<Command>
    {

        #region Fields

        private bool _pipelineComplete;
        private readonly CancellationToken _cancellationToken;
        private readonly List<IMiddleware<Command>> _middlewares;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="CommandPipeline"/>.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public CommandPipeline(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _middlewares = new List<IMiddleware<Command>>();
        }

        #endregion

        #region Public Methods

        public async Task Execute(Command input)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            foreach (IMiddleware<Command> middleware in _middlewares)
                await middleware.Invoke(input, _middlewares.NextOrDefault(middleware), _cancellationToken);

            await Task.CompletedTask;
        }
        public void Run(IMiddleware<Command> middleware)
        {
            Validate();
            _middlewares.Add(middleware);
            _pipelineComplete = true;
        }
        public IPipeline<Command> Use(IMiddleware<Command> middleware)
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
