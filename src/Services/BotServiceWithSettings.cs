using Freya.Core;
using Freya.Runtime;

using Mauve.Runtime;

namespace Freya.Services
{
    internal abstract class BotService<TSettings> : BotService
    {

        #region Properties

        protected TSettings Settings { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public BotService(TSettings settings, ILogger<LogEntry> logger, CommandFactory commandFactory, CancellationToken cancellationToken) :
            base(logger, commandFactory, cancellationToken) =>
            Settings = settings;

        #endregion

    }
}
