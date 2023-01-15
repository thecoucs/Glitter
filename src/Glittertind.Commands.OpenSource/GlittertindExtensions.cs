using Microsoft.Extensions.DependencyInjection;

namespace Glittertind.Commands.OpenSource
{
    public static class GlittertindExtensions
    {
        /// <summary>
        /// Adds open source commands to the DI container.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static GlittertindConfigurationBuilder AddOpenSourceCommands(this GlittertindConfigurationBuilder config) =>
            AddOpenSourceCommands<SessionData>(config);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static GlittertindConfigurationBuilder AddOpenSourceCommands<TSessionData>(this GlittertindConfigurationBuilder config)
            where TSessionData : SessionData, new() =>
            config.AddServices(services => services.AddSingleton<SessionData>(new TSessionData()));
    }
}
