using Newtonsoft.Json;

namespace Freya.Services.Discord
{
    /// <summary>
    /// Represents a collection of settings for the <see cref="DiscordService"/>.
    /// </summary>
    internal class DiscordSettings
    {
        /// <summary>
        /// The token used to authenticate with Discord.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;
    }
}
