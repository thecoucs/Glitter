using Freya.Commands;
using Freya.Core;
using Freya.Runtime;

using Mauve;
using Mauve.Runtime;

using MediatR;

namespace Freya.Services
{
    /// <summary>
    /// Represents a test service for testing within the deployed console.
    /// </summary>
    [Alias("test")]
    internal class TestChatbot : Chatbot
    {
        /// <summary>
        /// Creates a new <see cref="TestChatbot"/> instance.
        /// </summary>
        /// <param name="commandFactory">The command factory for the service to create commands.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public TestChatbot(
            RequestParser parser,
            ILogger<LogEntry> logger,
            IMediator mediator,
            CancellationToken cancellationToken) :
            base("Test", parser, logger, mediator, cancellationToken)
        { }
        /// <inheritdoc/>
        protected override async Task Run(CancellationToken cancellationToken)
        {
            // Cancel if requested, otherwise start the service.
            cancellationToken.ThrowIfCancellationRequested();
            await Log(EventType.Success, "Connected to console.");

            do
            {
                // Cancel if requested, otherwise wait for input.
                cancellationToken.ThrowIfCancellationRequested();
                string? input = Console.ReadLine();
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
                                    Console.WriteLine(response.Message);
                            }
                        }
                    }
                }
            } while (true);
        }
    }
}
