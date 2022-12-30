using Freya;

using Microsoft.Extensions.Configuration;

Console.WriteLine($"Starting Freya.");

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
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(parentDirectory.FullName)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.development.json", optional: true)
    .Build();

// Create the brain and wake it up.
var brain = new Brain();
brain.Configure(configuration);
await brain.Start();