using Glitter.Commands.OpenSource.General;

using Microsoft.Extensions.DependencyInjection;

namespace Glitter.Commands.OpenSource;

public static class GlitterExtensions
{
    /// <summary>
    /// Adds the core open source commands to the DI container.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static RuntimeOptionsBuilder AddOpenSourceCommands(this RuntimeOptionsBuilder specs) =>
        AddOpenSourceCommands<SessionData>(specs);
    /// <summary>
    /// Adds the core open source commands to the DI container.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="config"></param>
    /// <returns></returns>
    public static RuntimeOptionsBuilder AddOpenSourceCommands<TSessionData>(this RuntimeOptionsBuilder specs)
        where TSessionData : SessionData, new() =>
        specs.AddServices(services => services.AddSingleton<SessionData>(new TSessionData()))
              .WithSlashCommand<UptimeCommand>()
              .WithSlashCommand<VersionCommand>();
}
