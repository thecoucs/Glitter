using Freya.Core;
using Freya.Runtime;

using Mauve;
using Mauve.Runtime.Processing;

using Microsoft.Extensions.DependencyInjection;

namespace Freya.Services
{
    [Alias("test")]
    internal class TestService : BotService
    {

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public TestService(CommandFactory commandFactory, CancellationToken cancellationToken) :
            base("Test", new ConsoleLogger(), commandFactory, cancellationToken)
        { }

        #endregion

        #region Protected Methods

        protected override void ConfigureService(IServiceCollection services, IPipeline<BotCommand> pipeline)
        {

        }
        /// <inheritdoc/>
        protected override async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Log(EventType.Information, "The test service is alive.");
        }

        #endregion

    }
}
