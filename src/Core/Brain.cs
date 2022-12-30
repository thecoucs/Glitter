using Freya.Pipeline;
using Freya.Runtime;
using Freya.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Freya.Core
{
    internal class Brain
    {
        private readonly List<BotService> _bots;
        private readonly ServiceFactory _serviceFactory;
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;
        public Brain(ServiceFactory serviceFactory)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _bots = new List<BotService>();
            _serviceFactory = serviceFactory;
        }
        public void Cancel() =>
            _cancellationTokenSource.Cancel();
        public void Configure(IServiceCollection services)
        {
            // Cancel if requested, otherwise discover and configure services.
            _cancellationToken.ThrowIfCancellationRequested();
            foreach (BotService bot in _serviceFactory.Discover(_cancellationToken))
            {
                bot.Configure(services, new CommandPipeline(_cancellationToken));
                _bots.Add(bot);
            }
        }
        public async Task Start()
        {
            // Cancel if requested, otherwise start each service.
            _cancellationToken.ThrowIfCancellationRequested();
            foreach (BotService bot in _bots)
            {
                // Cancel if requested, otherwise start the service.
                _cancellationToken.ThrowIfCancellationRequested();
                await bot.Start();
            }

            await Task.Delay(Timeout.Infinite);
        }
    }
}
