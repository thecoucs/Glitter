using Freya.Commands;
using Mauve.Patterns;

namespace Freya.Pipeline
{
    /// <summary>
    /// Represents an implementation of <see cref="IMiddleware{T}"/> that injects dependencies into <see cref="Command"/> instances.
    /// </summary>
    /// <inheritdoc/>
    internal class DependencyInjectionMiddleware : IMiddleware<Command>
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
        public void Invoke(Command input, MiddlewareDelegate<Command> next)
        {

        }
        /// <inheritdoc/>
        public void Invoke(Command input, IMiddleware<Command> next) => throw new NotImplementedException();
        /// <inheritdoc/>
        public Task Invoke(Command input, IMiddleware<Command> next, CancellationToken cancellationToken) => throw new NotImplementedException();
        /// <inheritdoc/>
        public Task Invoke(Command input, MiddlewareDelegate<Command> next, CancellationToken cancellationToken) => throw new NotImplementedException();

        #endregion

        #region Private Methods

        #endregion

    }
}