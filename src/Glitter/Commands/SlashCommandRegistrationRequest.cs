using MediatR;

namespace Glitter.Commands;

/// <summary>
/// Represents a request to register a <see cref="SlashCommand"/>.
/// </summary>
public sealed class SlashCommandRegistrationRequest : IRequest
{
    /// <summary>
    /// Gets the <see cref="SlashCommand"/> to register.
    /// </summary>
    public SlashCommand Command { get; }
    /// <summary>
    /// Creates a new <see cref="SlashCommandRegistrationRequest"/> instance.
    /// </summary>
    /// <param name="command">The <see cref="SlashCommand"/> to register.</param>
    public SlashCommandRegistrationRequest(SlashCommand command) =>
        Command = command;
}
