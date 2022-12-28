using Freya;

Console.WriteLine($"Starting Freya.");

// Create the brain and wake it up.
var brain = new Brain();
brain.Initialize();
await brain.Start();