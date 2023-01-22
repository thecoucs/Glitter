namespace Glitter.Ai
{
    /// <summary>
    /// Defines the basic options for a <see cref="Chatbot"/>.
    /// </summary>
    public class ChatbotOptions
    {
        /// <summary>
        /// The prefix utilized to identify commands in a text-only based chat system.
        /// </summary>
        public string CommandPrefix { get; set; }
        /// <summary>
        /// The separator utilized to identify command arguments in a text-only based chat system.
        /// </summary>
        public string ParameterSeparator { get; set; }
        /// <summary>
        /// Creates a new <see cref="ChatbotOptions"/> instance.
        /// </summary>
        public ChatbotOptions()
        {
            CommandPrefix = "!";
            ParameterSeparator = ",";
        }
    }
}
