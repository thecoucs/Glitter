using Freya.Core;

using Mauve.Patterns;

namespace Freya.Pipeline
{
    internal class CommandExecutionMiddleware : IMiddleware<IBotCommand>
    {
        public void Invoke(IBotCommand input, MiddlewareDelegate<IBotCommand> next) => throw new NotImplementedException();
        public Task InvokeAsync(IBotCommand input, MiddlewareDelegate<IBotCommand> next) => throw new NotImplementedException();
        public Task InvokeAsync(IBotCommand input, MiddlewareDelegate<IBotCommand> next, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
