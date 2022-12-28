using Freya.Core;
using Freya.Services;

namespace Freya
{
    internal class Brain
    {

        #region Fields

        private readonly List<BotService> _services;
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Constructor

        public Brain()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _services = new List<BotService>();
        }

        #endregion

        #region Public Methods

        public void Cancel() =>
            _cancellationTokenSource.Cancel();
        public void Initialize() =>
            _services.Add(new TestService(_cancellationToken));
        public async Task Start()
        {
            // Cancel if requested, otherwise start each service.
            _cancellationToken.ThrowIfCancellationRequested();
            foreach (BotService service in _services)
            {
                // Cancel if requested, otherwise start the service.
                _cancellationToken.ThrowIfCancellationRequested();
                await service.Start();
            }
        }

        #endregion

    }
}
