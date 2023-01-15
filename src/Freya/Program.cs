using Freya.Extensibility;
using Freya.Logging;
using Freya.Providers.Discord;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Set the title of the debugging console.
Console.Title = "Freya";

// Create a host and start running it.
using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services => services
        .AddLogging(BuildLogging)
        .UseFreya(synapses => synapses
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