using Freya.Pipeline;
using Freya.Runtime;
using Freya.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Freya.Core
{
    /// <summary>
    /// Represents Freya's central processing system.
    /// </summary>
    internal class Brain
    {
        private readonly List<BotService> _bots;
        private readonly ServiceFactory _serviceFactory;
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;
        /// <summary>
        /// Creates a new instance of <see cref="Brain"/>.
        /// </summary>
        /// <param name="serviceFactory">The <see cref="ServiceFactory"/> for discovering <see cref="BotService"/> instances.</param>
        public Brain(ServiceFactory serviceFactory)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _bots = new List<BotService>();
            _serviceFactory = serviceFactory;
        }
        /// <summary>
        /// Cancels processing for the <see cref="Brain"/>.
        /// </summary>
        public void Cancel() =>
            _cancellationTokenSource.Cancel();
        /// <summary>
        /// Configures the <see cref="Brain"/> and discovered <see cref="BotService"/> instances.
        /// </summary>
        /// <param name="services">The service collection the <see cref="Brain"/> and <see cref="BotService"/> instances can add to.</param>
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
        /// <summary>
        /// Starts the brain.
        /// </summary>
        /// <returns>A <see cref="Task"/> describing the state of the operation.</returns>
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
