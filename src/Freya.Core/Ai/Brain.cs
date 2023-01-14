using Freya.Pipeline;
using Freya.Services;

using Microsoft.Extensions.Configuration;

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
            // Cancel if requested, otherwise load the application configuration.
            _cancellationToken.ThrowIfCancellationRequested();
            LoadConfiguration(out IConfiguration configuration);

            // Cancel if requested, otherwise discover and start each service.
            _cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine("Discovering providers.");
            var serviceFactory = new ChatbotFactory(configuration, _serviceProvider);
            foreach (Chatbot bot in serviceFactory.Discover(_cancellationToken))
            {
                // Cancel if requested, otherwise start the service.
                _cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine($"Starting provider {bot.Name}");
                bot.Configure(new CommandPipeline(_cancellationToken));
                await bot.Start(_cancellationToken);
            }

            await Task.Delay(Timeout.Infinite);
        }
        /// <summary>
        /// Loads the configuration for the application.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> for the application.</param>
        /// <exception cref="InvalidOperationException">Thrown when <see cref="AppContext.BaseDirectory"/> unable to be resolved.</exception>
        private void LoadConfiguration(out IConfiguration configuration)
        {
            // Validate the base directory.
            string baseDirectory = AppContext.BaseDirectory;
            if (string.IsNullOrWhiteSpace(baseDirectory))
                throw new InvalidOperationException("Unable to start. The base directory is null or empty.");

            // Validate the parent directory.
            DirectoryInfo? parentDirectory = Directory.GetParent(baseDirectory);
            if (parentDirectory is null)
                throw new InvalidOperationException("Unable to start. The parent directory is null.");

            // Build configuration
            Console.WriteLine("Building configuration.");
            configuration = new ConfigurationBuilder()
                .SetBasePath(parentDirectory.FullName)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", optional: true)
                .Build();
        }
    }
}
