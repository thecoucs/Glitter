using Freya.Core;
using Freya.Runtime;

using Mauve;
using Mauve.Patterns;
using Mauve.Runtime.Processing;

namespace Freya.Services
{
    internal class DiscordService : BotService
    {

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public DiscordService(CancellationToken cancellationToken) :
            base(new ConsoleLogger(), cancellationToken)
        { }

        #endregion

        #region Public Methods

        public override void Configure(IDependencyCollection dependencies, IPipeline<BotCommand> pipeline)
        {

        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override async Task Run(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Log(EventType.Information, "The test service is alive.");
        }

        #endregion

    }
}
