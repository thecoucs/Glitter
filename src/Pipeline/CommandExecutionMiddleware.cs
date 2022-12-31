using Freya.Commands;

using Mauve.Patterns;

namespace Freya.Pipeline
{
    /// <summary>
    /// Represents an implementation of <see cref="IMiddleware{T}"/> that executes <see cref="Command"/> instances.
    /// </summary>
    /// <inheritdoc/>
    internal class CommandExecutionMiddleware : IMiddleware<Command>
    {
        private readonly CancellationToken _cancellationToken;
        /// <summary>
        /// Creates a new instance of <see cref="CommandExecutionMiddleware"/>.
        /// </summary>
        public CommandExecutionMiddleware(CancellationToken cancellationToken) =>
            _cancellationToken = cancellationToken;
        /// <inheritdoc/>
        public async Task Invoke(Command input, IMiddleware<Command> next) =>
            _ = await input.Execute(_cancellationToken);
        /// <inheritdoc/>
        public async Task Invoke(Command input, IMiddleware<Command> next, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _ = await input.Execute(cancellationToken);
        }
    }
}
