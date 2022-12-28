using Freya.Core;
using Freya.Runtime;

using Mauve;
using Mauve.Patterns;
using Mauve.Runtime.Processing;

namespace Freya.Services
{
    internal class TestService : BotService
    {

        #region Constructor

        public TestService() :
            base(new ConsoleLogger())
        { }

        #endregion

        #region Public Methods

        public override void Configure(IDependencyCollection dependencies, IPipeline<BotCommand> pipeline)
        {

        }

        #endregion

        #region Protected Methods

        protected override async Task Run()
        {
            Log(EventType.Information, "The test service is alive.");
            await Task.Delay(Timeout.Infinite);
        }

        #endregion

    }
}
