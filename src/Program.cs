using System.Reflection;

using Freya.Core;
using Freya.Runtime;

using Mauve.Runtime;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Freya";
Console.WriteLine($"Initializing.");

// Create a new brain and host.
Console.WriteLine("Creating host.");
using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        // Add services.
        _ = services.AddMediatR(Assembly.GetExecutingAssembly())
                    .AddSingleton(new RequestParser("!", ","))
                    .AddSingleton<ILogger<LogEntry>>(new ConsoleLogger());

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