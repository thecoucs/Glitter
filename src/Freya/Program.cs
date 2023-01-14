using System.Reflection;

using Freya.Ai;
using Freya.Configuration;
using Freya.Core;
using Freya.Logging;
using Freya.Providers.Console;
using Freya.Providers.Discord;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Console.Title = "Freya";
Console.WriteLine($"Initializing.");

// Create a new brain and host.
Console.WriteLine("Creating host.");
using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        // Add services.
        _ = services.LoadConfiguration(out IConfiguration configuration)
            .AddLogging(BuildLogging)
            .AddMediatR(assemblies: Assembly.GetExecutingAssembly())
            .AddSingleton(new RequestParser(commandToken: "!", separator: ","))
            .AddTestingConsole()
            .AddDiscord(configuration);

        // Create a provider and register it for the command factory.
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        _ = services.AddSingleton(serviceProvider);
    })
    .Build();

// Signal that initialization is complete.
ConsoleColor initialColor = Console.ForegroundColor;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Initialization complete.");
Console.ForegroundColor = initialColor;

// Wake the brain up.
Console.WriteLine("Starting services.");
var brain = new Brain(host.Services);
await brain.Start();

// Builds the logging service.
static void BuildLogging(ILoggingBuilder logging) =>
    logging.AddDebug()
           .AddColoredConsole();