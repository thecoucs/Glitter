using Freya.Core;

using Mauve.Patterns;

namespace Freya.Pipeline
{
    /// <summary>
    /// Represents an implementation of <see cref="IMiddleware{T}"/> that injects dependencies into <see cref="BotCommand"/> instances.
    /// </summary>
    /// <inheritdoc/>
    internal class DependencyInjectionMiddleware : IMiddleware<BotCommand>
    {

        #region Fields

        private readonly IDependencyCollection _dependencies;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="DependencyInjectionMiddleware"/>.
        /// </summary>
        public DependencyInjectionMiddleware(IDependencyCollection dependencies) =>
            _dependencies = dependencies;

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Invoke(BotCommand input, MiddlewareDelegate<BotCommand> next)
        {

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