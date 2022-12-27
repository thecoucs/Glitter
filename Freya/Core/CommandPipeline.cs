using Mauve.Patterns;
using Mauve.Runtime.Processing;

namespace Freya.Core
{
    internal class CommandPipeline : IPipeline<IBotCommand>
    {
        public void Run(IMiddleware<IBotCommand> middleware) => throw new NotImplementedException();
        public IPipeline<IBotCommand> Use(IMiddleware<IBotCommand> middleware) => throw new NotImplementedException();
    }
}
