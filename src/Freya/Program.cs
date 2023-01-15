using Freya.Logging;

using Glittertind.Discord;

using Glittertind.Extensibility;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Set the title of the debugging console.
Console.Title = "Freya";

// Create a host and start running it.
using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services => services
        .AddLogging(BuildLogging)
        .UseGlittertind(config => config
            .EnableTesting()
            .SetCommandPrefix("!")
            .SetCommandSeparator(",")
            .AddDiscord())
).Build();
await host.RunAsync();

// Builds the logging service.
static void BuildLogging(ILoggingBuilder logging) =>
    logging.ClearProviders()
           .AddDebug()
           .AddColoredConsole()
           .SetMinimumLevel(LogLevel.Debug);