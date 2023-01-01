using Freya.Core;
using MediatR;

using Microsoft.Extensions.Logging;

namespace Freya.Services
{
    /// <summary>
    /// Represents a <see cref="Chatbot"/> with settings.
    /// </summary>
    /// <typeparam name="TSettings">Specifies the type used for settings.</typeparam>
    internal abstract class Chatbot<TSettings> : Chatbot
    {
        /// <summary>
        /// The settings for the service.
        /// </summary>
        protected TSettings Settings { get; set; }
        /// <summary>
        /// Creates a new <see cref="Chatbot{TSettings}"/> instance.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="settings">The settings for the service.</param>
        /// <param name="logger">The logger to be utilized by the service.</param>
        /// <param name="commandFactory">The <see cref="Commands.CommandRequestHandler"/> instance the service should use when creating commands.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
        public Chatbot(
            string name,
            TSettings settings,
            RequestParser parser,
            ILogger logger,
            IMediator mediator,
            CancellationToken cancellationToken) :
            base(name, parser, logger, mediator, cancellationToken) =>
            Settings = settings;
    }
}
