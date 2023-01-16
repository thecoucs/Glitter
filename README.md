# 🗻 Welcome aboard!
Named after the second largest mountain in Norway, Glitter is a framework for quickly building out chatbots and the commands they can invoke.

![.NET Workflow](https://github.com/tacosontitan/Glitter/workflows/.NET/badge.svg)

### Want to help out?
Get started by reviewing the answers to the following questions:
- [How can I help?](./CONTRIBUTING.md)
- [How should I behave here?](./CODE_OF_CONDUCT.md)
- [How do I report security concerns?](./SECURITY.md)

## 🎉 Create your first chatbot!
It's never been easier to get a custom made chatbot up and running. Simply add a few lines of code to your `Program.cs` file to get started:
```csharp
using Glitter;
using Glitter.Discord;
using Glitter.Extensibility;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services => services
        .UseGlitter(config => config
            .EnableTesting()
            .SetCommandPrefix("!")
            .SetCommandSeparator(",")
            .AddOpenSourceCommands()
            .AddDiscord("<AUTH_TOKEN>"))
).Build();
await host.RunAsync();
```
With that, you've got a working chatbot that can communicate with Discord; and all that's left is to add some custom commands!

# 💪 Powered by Open Source
The following open source projects help to power both Glitter and Freya; be sure to give them a star! ⭐
 - [Mauve](https://github.com/tacosontitan/Mauve)
 - [MediatR](https://github.com/jbogard/MediatR)
 - [TwitchLib](https://github.com/TwitchLib/TwitchLib)

## Open Source Commands
There are endless possibilities for chatbot commands! Glitter encourages the open-source community to create a vast global baseline of commands for public consumption. Here's our current baseline list of commands included with all Glitter powered bots:

- `help`: Queries for a list of commands.
- `version`: Queries for the current version of the bot.
- `uptime`: Queries the time the bot has been up and running.

Of course, these are just the commands that are automatically registered with your bot through `AddOpenSourceCommands`. You can manually register any open-source command just like you would a custom made one:
```csharp
services.UseGlitter(config => config.AddCommand<MySampleCommand>());
```

# 🧪 Tested by Open Source
The following open source projects help to keep Glitter tested and held to the highest standards; be sure to give them a star! ⭐
 - [Fluent Assertions](https://github.com/fluentassertions/fluentassertions)
 - [Fluent Validation](https://github.com/FluentValidation/FluentValidation)
 - [Shouldly](https://github.com/shouldly/shouldly)

# 💃 Freya
Freya is [tacosontitan's](https://github.com/tacosontitan) personal chatbot built using Glitter as a design pallete and proof of concept.

![Freya Header](/.resources/freya-header.png "Freya Header")
