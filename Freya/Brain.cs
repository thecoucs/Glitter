using Freya.Core;

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

    }
}
