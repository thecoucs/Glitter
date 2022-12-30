using Freya.Commands;
using Mauve.Patterns;

namespace Freya.Pipeline
{
    /// <summary>
    /// Represents an implementation of <see cref="IMiddleware{T}"/> that executes <see cref="BotCommand"/> instances.
    /// </summary>
    /// <inheritdoc/>
    internal class CommandExecutionMiddleware : IMiddleware<BotCommand>
    {

        #region Fields

        private readonly CancellationToken _cancellationToken;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="CommandExecutionMiddleware"/>.
        /// </summary>
        public CommandExecutionMiddleware(CancellationToken cancellationToken) =>
            _cancellationToken = cancellationToken;

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Invoke(BotCommand input, MiddlewareDelegate<BotCommand> next) =>
            input.Execute(_cancellationToken).GetAwaiter();
        /// <inheritdoc/>
        public void Invoke(BotCommand input, IMiddleware<BotCommand> next) =>
            input.Execute(_cancellationToken).GetAwaiter();
        /// <inheritdoc/>
        public async Task Invoke(BotCommand input, IMiddleware<BotCommand> next, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await input.Execute(cancellationToken);
        }
        /// <inheritdoc/>
        public async Task Invoke(BotCommand input, MiddlewareDelegate<BotCommand> next, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await input.Execute(cancellationToken);
        }

        #endregion

    }
}
