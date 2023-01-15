//using Freya.Pipeline;
//using Freya.Ai;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;

//namespace Freya.Ai
//{
//    /// <summary>
//    /// Represents Freya's central processing system.
//    /// </summary>
//    public class Brain : BackgroundService
//    {
//        private readonly ILogger _logger;
//        private readonly IServiceProvider _serviceProvider;
//        /// <summary>
//        /// Creates a new instance of <see cref="Brain"/>.
//        /// </summary>
//        /// <param name="serviceFactory">The <see cref="ChatbotFactory"/> for discovering <see cref="Chatbot"/> instances.</param>
//        public Brain(IServiceProvider serviceProvider, ILogger<Brain> logger)
//        {
//            _logger = logger;
//            _serviceProvider = serviceProvider;
//        }
//        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
//        {
//            // Cancel if requested, otherwise discover and start each service.
//            cancellationToken.ThrowIfCancellationRequested();
//            _logger.LogDebug("Discovering providers.");
//            foreach (Chatbot bot in _serviceProvider.GetServices<Chatbot>())
//            {
//                // Cancel if requested, otherwise start the service.
//                cancellationToken.ThrowIfCancellationRequested();
//                _logger.LogDebug($"Starting provider {bot.Name}");
//                bot.Configure(new CommandPipeline(cancellationToken));
//                await bot.Start(cancellationToken);
//            }
//        }
//    }
//}
