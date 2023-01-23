using Glitter.Commands.OpenSource.General;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Glitter.Commands.OpenSource;

public static class GlitterExtensions
{
    /// <summary>
    /// Adds the core open source commands to the DI container.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static OptionsBuilder<GlitterOptions> AddOpenSourceCommands(this OptionsBuilder<GlitterOptions> optionsBuilder) =>
        AddOpenSourceCommands<SessionData>(optionsBuilder);
    /// <summary>
    /// Adds the core open source commands to the DI container.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="config"></param>
    /// <returns></returns>
    public static OptionsBuilder<GlitterOptions> AddOpenSourceCommands<TSessionData>(this OptionsBuilder<GlitterOptions> optionsBuilder)
        where TSessionData : SessionData, new()
    {
        _ = optionsBuilder.AddSlashCommand<UptimeCommand>()
                          .AddSlashCommand<VersionCommand>()
                          .Services.AddSingleton<SessionData>(new TSessionData());

        return optionsBuilder;
    }
}
