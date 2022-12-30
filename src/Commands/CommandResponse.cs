namespace Freya.Commands
{
    /// <summary>
    /// Represents a <see cref="CommandResponse"/>.
    /// </summary>
    internal sealed class CommandResponse
    {
        /// <summary>
        /// The data in the response.
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// Creates a new instance of <see cref="CommandResponse"/>.
        /// </summary>
        /// <param name="data">The data in the response.</param>
        public CommandResponse(byte[] data) =>
            Data = data;
    }
}
