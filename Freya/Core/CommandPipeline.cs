using Mauve.Patterns;
using Mauve.Runtime.Processing;

namespace Freya.Core
{
    internal class CommandPipeline : IPipeline<BotCommand>
    {
        public void Run(IMiddleware<BotCommand> middleware) => throw new NotImplementedException();
        public IPipeline<BotCommand> Use(IMiddleware<BotCommand> middleware) => throw new NotImplementedException();
    }
}
