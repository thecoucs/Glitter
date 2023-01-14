using Freya.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Freya.Providers.Console
{
    public static class ConsoleServiceExtensions
    {
        public static IServiceCollection AddTestingConsole(this IServiceCollection services) =>
            services.AddSingleton<Chatbot, ConsoleChatbot>();
    }
}