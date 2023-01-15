using Glittertind.Commands;

using Mauve.Patterns;

namespace Glittertind.Pipeline
{
    /// <summary>
    /// Represents an implementation of <see cref="IMiddleware{T}"/> that executes <see cref="Command"/> instances.
    /// </summary>
    /// <inheritdoc/>
    public class CommandExecutionMiddleware : IMiddleware<Command>
    {
        /// <inheritdoc/>
        public async Task Invoke(Command input, IMiddleware<Command> next) =>
            _ = await input.Execute(CancellationToken.None);
        /// <inheritdoc/>
        public async Task Invoke(Command input, IMiddleware<Command> next, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _ = await input.Execute(cancellationToken);
        }
    }
}
