using Freya.Commands;
using Freya.Core;
using Freya.Services;

using Mauve;

using MediatR;

using Microsoft.Extensions.Logging;

using SystemConsole = System.Console;

namespace Freya.Providers.Console
{
    /// <summary>
    /// Represents a test service for testing within the deployed console.
    /// </summary>
    [Alias("test")]
    internal class ConsoleChatbot : Chatbot
    {
        /// <summary>
        /// Creates a new <see cref="ConsoleChatbot"/> instance.
        /// </summary>
        /// <param name="commandFactory">The command factory for the service to create commands.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public ConsoleChatbot(
            RequestParser parser,
            ILogger<ConsoleChatbot> logger,
            IMediator mediator) :
            base("Test", parser, logger, mediator)
        { }
        /// <inheritdoc/>
        protected override async Task Run(CancellationToken cancellationToken)
        {
            // Cancel if requested, otherwise start the service.
            cancellationToken.ThrowIfCancellationRequested();
            Log(LogLevel.Information, "Connected to console.");

            do
            {
                // Cancel if requested, otherwise wait for input.
                cancellationToken.ThrowIfCancellationRequested();
                string? input = SystemConsole.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (Parser.TryParse(input, out CommandRequest? commandRequest))
                    {
                        if (commandRequest is not null)
                        {
                            Command? command = await Mediator.Send(commandRequest, cancellationToken);
                            if (command is not null)
                            {
                                CommandResponse? response = await command.Execute(cancellationToken);
                                if (!string.IsNullOrWhiteSpace(response?.Message))
                                    SystemConsole.WriteLine(response.Message);
                            }
                        }
                    }
                }
            } while (true);
        }
    }
}
