namespace Freya.Commands
{
    /// <summary>
    /// Represents a command request within Freya.
    /// </summary>
    internal record CommandRequest
    {
        /// <summary>
        /// The key used for invoking the command.
        /// </summary>
        public string Key { get; init; }
        /// <summary>
        /// The parameters provided for the command.
        /// </summary>
        public IEnumerable<object> Parameters { get; init; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRequest"/> class.
        /// </summary>
        /// <param name="key">The key used for invoking the command.</param>
        /// <param name="parameters">The parameters provided for the command.</param>
        public CommandRequest(string key, params object[] parameters)
        {
            Key = key;
            Parameters = parameters;
        }
    }
}
