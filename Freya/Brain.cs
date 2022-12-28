using Freya.Core;
using Freya.Services;

namespace Freya
{
    internal class Brain
    {

        #region Fields

        private readonly List<BotService> _services;

        #endregion

        #region Constructor

        public Brain() =>
            _services = new List<BotService>();

        #endregion

        #region Public Methods

        public void Initialize() =>
            _services.Add(new TestService());
        public async Task Start()
        {
            // Start all of the bot services.
            foreach (BotService service in _services)
                await service.Start();
        }

        #endregion

    }
}
