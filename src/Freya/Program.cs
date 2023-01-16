using Freya.Logging;

using Glitter;
using Glitter.Commands.OpenSource;
using Glitter.Discord;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Set the title of the debugging console.
Console.Title = "Freya";

// Create a host and start running it.
using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services => services
        .AddLogging(BuildLogging)
        .UseGlitter(config => config
            .EnableTesting()
            .SetCommandPrefix("!")
            .SetCommandSeparator(",")
            .AddOpenSourceCommands()
            .AddDiscord())
).Build();
await host.RunAsync();

// Builds the logging service.
static void BuildLogging(ILoggingBuilder logging) =>
    logging.ClearProviders()
           .AddDebug()
           .AddColoredConsole()
           .SetMinimumLevel(LogLevel.Debug);