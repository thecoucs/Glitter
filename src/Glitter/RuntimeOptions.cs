namespace Glitter;

internal sealed class RuntimeOptions
{
    /// <summary>
    /// Specifies whether or not a <see cref="Console"/> driven bot is available for testing purposes.
    /// </summary>
    public bool TestBotEnabled { get; set; }
    /// <summary>
    /// The prefix utilized to identify commands in a text-only based chat system.
    /// </summary>
    public string? CommandPrefix { get; set; }
    /// <summary>
    /// The separator utilized to identify command arguments in a text-only based chat system.
    /// </summary>
    public string? CommandSeparator { get; set; }
    /// <summary>
    /// A collection of <see cref="Type"/>s for registering commands with providers.
    /// </summary>
    public List<Type>? CommandTypes { get; set; }
    /// <summary>
    /// Creates a new <see cref="RuntimeOptions"/> instance.
    /// </summary>
    public RuntimeOptions()
    {
        TestBotEnabled = false;
        CommandPrefix = "!";
        CommandSeparator = ",";
        CommandTypes = new List<Type>();
    }
}
