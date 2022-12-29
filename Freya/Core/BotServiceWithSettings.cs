using Mauve.Runtime;

namespace Freya.Core
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
        public BotService(TSettings settings, ILogger<LogEntry> logger, CancellationToken cancellationToken) :
            base(logger, cancellationToken) =>
            Settings = settings;

        #endregion

    }
}
