using Freya.Core;

using Mauve.Patterns;

namespace Freya.Pipeline
{
    internal class CommandExecutionMiddleware : IMiddleware<BotCommand>
    {
        public void Invoke(BotCommand input, MiddlewareDelegate<BotCommand> next) => throw new NotImplementedException();
        public Task InvokeAsync(BotCommand input, MiddlewareDelegate<BotCommand> next) => throw new NotImplementedException();
        public Task InvokeAsync(BotCommand input, MiddlewareDelegate<BotCommand> next, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
