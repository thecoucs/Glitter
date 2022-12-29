using Mauve;

namespace Freya.Commands
{
    [Alias("remind")]
    internal class ReminderCommand
    {

        #region Fields

        private readonly string _what;
        private readonly DateTime _when;

        #endregion

        #region Constructor

        public ReminderCommand([FromBot] DateTime when, [FromBot] string what)
        {
            _when = when;
            _what = what;
        }

        #endregion

    }
}
