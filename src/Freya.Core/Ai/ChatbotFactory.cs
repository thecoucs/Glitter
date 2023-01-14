using System.Reflection;

using Freya.Commands;
using Freya.Core;

using Mauve;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freya.Services
{
    /// <summary>
    /// Represents a factory responsible for creating <see cref="Chatbot"/> instances.
    /// </summary>
    public class ChatbotFactory : AliasedTypeFactory<Chatbot>
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// Creates a new <see cref="ChatbotFactory"/> instance.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="commandFactory">The factory services should use when creating <see cref="Command"/> instances.</param>
        public ChatbotFactory(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// Discovers all concrete <see cref="Chatbot"/> implementations in the current <see cref="Assembly"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to be provided to the <see cref="Chatbot"/> upon creation.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing all concrete <see cref="Chatbot"/> implementations in the current <see cref="Assembly"/>.</returns>
        public IEnumerable<Chatbot> Discover(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                return _serviceProvider.GetServices<Chatbot>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Enumerable.Empty<Chatbot>();
        }
    }
}
