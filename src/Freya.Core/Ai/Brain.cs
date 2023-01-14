using Freya.Pipeline;
using Freya.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Freya.Ai
{
    /// <summary>
    /// Represents Freya's central processing system.
    /// </summary>
    public class Brain
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;
        /// <summary>
        /// Creates a new instance of <see cref="Brain"/>.
        /// </summary>
        /// <param name="serviceFactory">The <see cref="ChatbotFactory"/> for discovering <see cref="Chatbot"/> instances.</param>
        public Brain(IServiceProvider serviceProvider)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// Cancels processing for the <see cref="Brain"/>.
        /// </summary>
        public void Cancel() =>
            _cancellationTokenSource.Cancel();
        /// <summary>
        /// Starts the brain.
        /// </summary>
        /// <returns>A <see cref="Task"/> describing the state of the operation.</returns>
        public async Task Start()
        {
            // Cancel if requested, otherwise discover and start each service.
            _cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine("Discovering providers.");
            foreach (Chatbot bot in _serviceProvider.GetServices<Chatbot>())
            {
                // Cancel if requested, otherwise start the service.
                _cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine($"Starting provider {bot.Name}");
                bot.Configure(new CommandPipeline(_cancellationToken));
                await bot.Start(_cancellationToken);
            }

            // Wake up all event handlers.
            // TODO: This is a disgusting way to handle this, fix it per issue #6.
            _ = _serviceProvider.GetServices<IEventHandler>();
            await Task.Delay(Timeout.Infinite);
        }
    }
}
