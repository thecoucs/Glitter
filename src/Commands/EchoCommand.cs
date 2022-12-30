using System.Text;

using Discord.Commands;
using Freya.Core;
using Mauve.Runtime;

namespace Freya.Commands
{
    [Alias("echo")]
    internal class EchoCommand : Command
    {

        #region Fields

        private readonly string _input;

        #endregion

        #region Constructor

        public EchoCommand([FromBot] string input, ILogger<LogEntry> logger) :
            base("Echo", "Echoes the specified input back to the user.", logger) =>
            _input = input;

        #endregion

        #region Protected Methods

        protected override async Task Work(CancellationToken cancellationToken)
        {
            byte[] data = Encoding.UTF8.GetBytes(_input);
            _ = new CommandResponse(data);
            //await invokingService.SendResponse(response, cancellationToken);
            await Task.CompletedTask; // TODO: remove.
        }

        #endregion

    }
}
