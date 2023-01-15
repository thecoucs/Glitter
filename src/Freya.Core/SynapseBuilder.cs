namespace Freya
{
    /// <summary>
    /// Represents an interface that exposes methods to configure Freya.
    /// </summary>
    public class SynapseBuilder
    {
        /// <summary>
        /// Specifies whether or not a <see cref="Console"/> driven bot is available for testing purposes.
        /// </summary>
        internal bool TestBotEnabled { get; private set; }
        /// <summary>
        /// The prefix utilized to identify commands in a text-only based chat system.
        /// </summary>
        internal string? CommandPrefix { get; private set; }
        /// <summary>
        /// The separator utilized to identify command arguments in a text-only based chat system.
        /// </summary>
        internal string? CommandSeparator { get; private set; }
        /// <summary>
        /// Enables a <see cref="Console"/> driven bot for testing purposes.
        /// </summary>
        /// <returns>The current <see cref="SynapseBuilder"/> instance with the testing console enabled.</returns>
        public SynapseBuilder EnableTesting()
        {
            TestBotEnabled = true;
            return this;
        }
        /// <summary>
        /// Sets the prefix utilized to identify commands in a text-only based chat system.
        /// </summary>
        /// <param name="commandPrefix">The prefix utilized to identify commands in a text-only based chat system.</param>
        /// <returns>The current <see cref="SynapseBuilder"/> instance with the command prefix set to the specified value.</returns>
        public SynapseBuilder SetCommandPrefix(string commandPrefix)
        {
            CommandPrefix = commandPrefix;
            return this;
        }
        /// <summary>
        /// Sets the separator utilized to identify command arguments in a text-only based chat system.
        /// </summary>
        /// <param name="commandSeparator">The separator utilized to identify command arguments in a text-only based chat system.</param>
        /// <returns>The current <see cref="SynapseBuilder"/> instance with the command separator set to the specified value.</returns>
        public SynapseBuilder SetCommandSeparator(string commandSeparator)
        {
            CommandSeparator = commandSeparator;
            return this;
        }
    }
}
