using Freya.Core;

using Mauve.Patterns;

namespace Freya.Pipeline
{
    /// <summary>
    /// Represents an implementation of <see cref="IMiddleware"/> that...
    /// </summary>
    /// <inheritdoc/>
    internal class LoggingMiddleware : IMiddleware<IBotCommand>
    {

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="LoggingMiddleware"/>.
        /// </summary>
        public LoggingMiddleware()
        {

        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Invoke(IBotCommand input, MiddlewareDelegate<IBotCommand> next)
        {

        }
        /// <inheritdoc/>
        public async Task InvokeAsync(IBotCommand input, MiddlewareDelegate<IBotCommand> next) =>
            await InvokeAsync(input, next, CancellationToken.None);
        /// <inheritdoc/>
        public async Task InvokeAsync(IBotCommand input, MiddlewareDelegate<IBotCommand> next, CancellationToken cancellationToken) =>
            await Task.Run(() => Invoke(input, next), cancellationToken);

        #endregion

    }
}