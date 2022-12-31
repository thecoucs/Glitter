using Freya.Core;

using Mauve;
using Mauve.Runtime;

using MediatR;

namespace Freya.Services
{
    /// <summary>
    /// Represents a test service for testing within the deployed console.
    /// </summary>
    [Alias("test")]
    internal class TestService : Chatbot
    {
        /// <summary>
        /// Creates a new <see cref="TestService"/> instance.
        /// </summary>
        /// <param name="commandFactory">The command factory for the service to create commands.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public TestService(ILogger<LogEntry> logger, IMediator mediator, CancellationToken cancellationToken) :
            base("Test", logger, mediator, cancellationToken)
        { }
        /// <inheritdoc/>
        protected override async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Log(EventType.Information, "The test service is alive.");
        }
    }
}
