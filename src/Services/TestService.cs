using Freya.Commands;
using Freya.Runtime;

using Mauve;
using Mauve.Runtime.Processing;

using Microsoft.Extensions.DependencyInjection;

namespace Freya.Services
{
    /// <summary>
    /// Represents a test service for testing within the deployed console.
    /// </summary>
    [Alias("test")]
    internal class TestService : BotService
    {
        /// <summary>
        /// Creates a new <see cref="TestService"/> instance.
        /// </summary>
        /// <param name="commandFactory">The command factory for the service to create commands.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public TestService(CommandFactory commandFactory, CancellationToken cancellationToken) :
            base("Test", new ConsoleLogger(), commandFactory, cancellationToken)
        { }
        /// <inheritdoc/>
        protected override void ConfigureService(IServiceCollection services, IPipeline<Command> pipeline)
        {

        }
        /// <inheritdoc/>
        protected override async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Log(EventType.Information, "The test service is alive.");
        }
    }
}
