using System.Text;

namespace Glitter.Commands.OpenSource;

internal static class StringBuilderExtensions
{
    /// <summary>
    /// Appends the specified <paramref name="input"/> if the specified <paramref name="condition"/> is <see langword="true"/>.
    /// </summary>
    /// <param name="stringBuilder">The string builder to append to.</param>
    /// <param name="input">The input to append.</param>
    /// <param name="condition">The condition for determining if the input should be appeneded.</param>
    /// <returns>The current <see cref="StringBuilder"/> with the input appeneded or not.</returns>
    public static StringBuilder AppendIf(this StringBuilder stringBuilder, string input, bool condition) =>
        condition ? stringBuilder.Append(input) : stringBuilder;
}
