# 🗻 Glittertind
Named after the second largest mountain in Norway, Glittertind is a framework for quickly building out chatbots and the commands they can invoke.

## 🎉 Just a little demo
It's never been easier to get a custom made chatbot up and running. Simply add a few lines of code to your `Program.cs` file to get started:
```csharp
using Glittertind.Discord;
using Glittertind.Extensibility;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Create a host and start running it.
using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services => services
        .UseGlittertind(synapses => synapses
            .EnableTesting()
            .SetCommandPrefix("!")
            .SetCommandSeparator(",")
            .AddDiscord())
).Build();
await host.RunAsync();
```
With that, you've got a working chatbot that can communicate with Discord; and all that's left is to add some commands!

![Freya Header](/.resources/freya-header.png "Freya Header")
# 💃 Freya

 - General Commands
   - Remind Me
 - Development Commands
   - Pull Request
 - Financial Commands
   - Quote
   - Chart
   - Positions
   - Watchlist
   - Balance
# 💪 Powered By Open Source
 - [Mauve](https://github.com/tacosontitan/Mauve)
 - [MediatR](https://github.com/jbogard/MediatR)