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

                // Validate any input received.
                if (string.IsNullOrWhiteSpace(input) ||
                    !Parser.TryParse(input, out CommandRequest? commandRequest) ||
                    commandRequest is null)
                    continue;

                // Capture the command and validate.
                Command? command = await Mediator.Send(commandRequest, cancellationToken);
                if (command is null)
                {
                    Log(LogLevel.Warning, $"Unable to locate a command that satisfies the given request.");
                    continue;
                }

                // Execute the command and handle the response.
                CommandResponse? response = await command.Execute(cancellationToken);
                if (!string.IsNullOrWhiteSpace(response?.Message))
                    SystemConsole.WriteLine(response.Message);
            } while (true);
        }
    }
}
