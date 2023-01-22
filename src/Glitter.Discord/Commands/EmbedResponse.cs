using Discord;

using Glitter.Commands;

namespace Glitter.Discord.Commands;

/// <summary>
/// Represents a <see cref="CommandResponse"/> associated with an <see cref="Embed"/>.
/// </summary>
public class EmbedResponse : CommandResponse
{
    /// <summary>
    /// The embed for the response.
    /// </summary>
    public Embed? Embed { get; set; }
    /// <summary>
    /// Creates a new <see cref="EmbedResponse"/> instance.
    /// </summary>
    /// <param name="message">The basic message for the response.</param>
    public EmbedResponse(string message) :
        base(message)
    { }
    /// <summary>
    /// Creates a new <see cref="EmbedResponse"/> instance.
    /// </summary>
    /// <param name="embed">The embed for the response.</param>
    /// <param name="message">The basic message for the response.</param>
    public EmbedResponse(Embed embed, string message) :
        base(message) =>
        Embed = embed;
}