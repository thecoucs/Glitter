using Glitter.Commands;

using Mauve.Extensibility;
using Mauve.Patterns;
using Mauve.Runtime;

namespace Glitter.Pipeline
{
    /// <summary>
    /// Represents an implementation of <see cref="IPipeline{T}"/> that executes <see cref="Command"/> instances.
    /// </summary>
    public class CommandPipeline : IPipeline<Command>
    {
        private bool _pipelineComplete;
        private readonly CancellationToken _cancellationToken;
        private readonly List<IMiddleware<Command>> _middlewares;
        /// <summary>
        /// Creates a new instance of <see cref="CommandPipeline"/>.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public CommandPipeline(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _middlewares = new List<IMiddleware<Command>>();
        }
        /// <summary>
        /// Executes the specified <see cref="Command"/> through the pipeline.
        /// </summary>
        /// <param name="input">The <see cref="Command"/> to execute.</param>
        /// <returns>A <see cref="Task"/> representing the state of the operation.</returns>
        public async Task Execute(Command input)
        {
            foreach (IMiddleware<Command> middleware in _middlewares)
            {
                _cancellationToken.ThrowIfCancellationRequested();
                await middleware.Invoke(input, _middlewares.NextOrDefault(middleware), _cancellationToken);
            }

            await Task.CompletedTask;
        }
        /// <summary>
        /// Executes the specified <see cref="Command"/> through the pipeline.
        /// </summary>
        /// <param name="input">The <see cref="Command"/> to execute.</param>
        /// <returns>A <see cref="Task"/> representing the state of the operation.</returns>
        public async Task Execute(Command input, CancellationToken cancellationToken)
        {
            foreach (IMiddleware<Command> middleware in _middlewares)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await middleware.Invoke(input, _middlewares.NextOrDefault(middleware), cancellationToken);
            }

            await Task.CompletedTask;
        }
        /// <summary>
        /// Specifies the final middleware in the pipeline.
        /// </summary>
        /// <param name="middleware">The middleware that should complete the pipeline.</param>
        public void Run(IMiddleware<Command> middleware)
        {
            Validate();
            _middlewares.Add(middleware);
            _pipelineComplete = true;
        }
        /// <summary>
        /// Specifies that a middleware should be added to the pipeline for use.
        /// </summary>
        /// <param name="middleware">The middleware to be added to the pipeline.</param>
        /// <returns>The current pipeline with the specified middleware added.</returns>
        public IPipeline<Command> Use(IMiddleware<Command> middleware)
        {
            Validate();
            _middlewares.Add(middleware);
            return this;
        }
        /// <summary>
        /// Validates the pipeline.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the pipeline is complete.</exception>
        private void Validate()
        {
            if (_pipelineComplete)
                throw new InvalidOperationException("The pipeline has already been completed.");
        }
    }
}
