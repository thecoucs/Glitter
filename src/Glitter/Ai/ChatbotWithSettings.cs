using MediatR;

using Microsoft.Extensions.Logging;

namespace Glitter.Ai
{
    /// <summary>
    /// Represents a <see cref="Chatbot"/> with settings.
    /// </summary>
    /// <typeparam name="TSettings">Specifies the type used for settings.</typeparam>
    public abstract class Chatbot<TSettings> : Chatbot
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
        public Chatbot(
            string name,
            TSettings settings,
            IMediator mediator,
            ILogger logger) :
            base(name, mediator, logger) =>
            Settings = settings;
    }
}
