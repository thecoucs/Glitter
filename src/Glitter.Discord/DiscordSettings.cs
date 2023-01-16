using Newtonsoft.Json;

namespace Glitter.Discord;

/// <summary>
/// Represents a collection of settings for the <see cref="DiscordChatbot"/>.
/// </summary>
internal class DiscordSettings
{
    /// <summary>
    /// The token used to authenticate with Discord.
    /// </summary>
    [JsonProperty("token")]
    public string Token { get; set; } = string.Empty;
}
