using Freya.Core;

using Mauve.Patterns;

namespace Freya.Pipeline
{
    /// <summary>
    /// Represents an implementation of <see cref="IMiddleware{T}"/> that executes <see cref="BotCommand"/> instances.
    /// </summary>
    /// <inheritdoc/>
    internal class CommandExecutionMiddleware : IMiddleware<BotCommand>
    {

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="CommandExecutionMiddleware"/>.
        /// </summary>
        public CommandExecutionMiddleware() { }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Invoke(BotCommand input, MiddlewareDelegate<BotCommand> next)
        {
            try
            {
                input.Execute();
            } catch { } finally
            {
                _ = next?.Invoke(input);
            }
        }
        /// <inheritdoc/>
        public async Task InvokeAsync(BotCommand input, MiddlewareDelegate<BotCommand> next) =>
            await InvokeAsync(input, next, CancellationToken.None);
        /// <inheritdoc/>
        public async Task InvokeAsync(BotCommand input, MiddlewareDelegate<BotCommand> next, CancellationToken cancellationToken) =>
            await Task.Run(() => Invoke(input, next), cancellationToken);

        #endregion

    }
}
