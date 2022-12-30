using Newtonsoft.Json;

namespace Freya.Services.Discord
{
    internal class DiscordSettings
    {
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;
    }
}
