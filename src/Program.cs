using Freya.Core;
using Freya.Runtime;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Freya";
Console.WriteLine($"Initializing.");

// Validate the base directory.
string baseDirectory = AppContext.BaseDirectory;
if (string.IsNullOrWhiteSpace(baseDirectory))
{
    Console.WriteLine("Unable to start. The base directory is null or empty.");
    return;
}

// Validate the parent directory.
DirectoryInfo? parentDirectory = Directory.GetParent(baseDirectory);
if (parentDirectory is null)
{
    Console.WriteLine("Unable to start. The parent directory is null.");
    return;
}

// Build configuration
Console.WriteLine("Building configuration.");
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(parentDirectory.FullName)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.development.json", optional: true)
    .Build();

// Create a new brain and host.
Brain brain = null;
Console.WriteLine("Creating host.");
using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        // Register the factories.
        Console.WriteLine("Registering factories.");
        var commandFactory = new CommandFactory();
        var serviceFactory = new ServiceFactory(configuration, commandFactory);
        _ = services.AddSingleton(serviceFactory)
                    .AddSingleton(commandFactory);

        // Configure the brain.
        Console.WriteLine("Configuring.");
        brain = new Brain(serviceFactory);
        brain.Configure(services);
    })
    .Build();

// Validate the brain.
if (brain is null)
{
    Console.WriteLine("Unable to start. The brain does not exist.");
    return;
}

// Signal that initialization is complete.
ConsoleColor initialColor = Console.ForegroundColor;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Initialization complete.");
Console.ForegroundColor = initialColor;

// Wake the brain up.
Console.WriteLine("Starting services.");
await brain.Start();