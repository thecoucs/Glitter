using Freya.Logging;

using Glitter;
using Glitter.Commands.OpenSource;
using Glitter.Discord;

// Set the title of the debugging console.
Console.Title = "Freya";

// Create a host and start running it.
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => services
        .AddLogging(BuildLogging)
        .AddGlitter()
        .AddOpenSourceCommands()
        .AddDiscord("<AUTH_TOKEN>")
        .EnableTesting(commandPrefix: "!", parameterSeparator: ",")
).Build();
await host.RunAsync();

// Builds the logging service.
static void BuildLogging(ILoggingBuilder logging) =>
    logging.ClearProviders()
           .AddDebug()
           .AddColoredConsole()
           .SetMinimumLevel(LogLevel.Debug);