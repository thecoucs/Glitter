using System.Reflection;

using Glitter.Ai;
using Glitter.Commands;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Glitter;

/// <summary>
/// Represents an interface that exposes methods to configure Freya.
/// </summary>
public class RuntimeOptionsBuilder
{
    private readonly List<Assembly> _chatbotAssemblies;
    private readonly IConfiguration _configuration;
    private readonly RuntimeOptions _options;
    private readonly IServiceCollection _services;
    /// <summary>
    /// Creates a new <see cref="RuntimeOptionsBuilder"/> instance.
    /// </summary>
    /// <param name="services">A service contract for registering services with the DI container.</param>
    /// <param name="configuration">The currently loaded configuration.</param>
    internal RuntimeOptionsBuilder(IServiceCollection services, IConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
        _options = new RuntimeOptions();
        _chatbotAssemblies = new List<Assembly>();
    }
    /// <summary>
    /// Adds a <see cref="Chatbot"/> to the DI container.
    /// </summary>
    /// <typeparam name="T">Specifies the type of <see cref="Chatbot"/> to add.</typeparam>
    /// <returns>The current <see cref="RuntimeOptionsBuilder"/> instance with the specified type added as a hosted service.</returns>
    public RuntimeOptionsBuilder AddChatbot<T>() where T : Chatbot
    {
        _ = _services.AddHostedService<T>();
        var chatbotAssembly = Assembly.GetAssembly(typeof(T));
        if (chatbotAssembly is not null)
            _chatbotAssemblies.Add(chatbotAssembly);

        return this;
    }
    /// <summary>
    /// Adds a <see cref="SlashCommand"/> to the registration queue.
    /// </summary>
    /// <typeparam name="T">Speicifies the type of <see cref="SlashCommand"/> to add.</typeparam>
    /// <returns>The current <see cref="RuntimeOptionsBuilder"/> instance with the specified type added to the registration queue.</returns>
    public RuntimeOptionsBuilder WithSlashCommand<T>() where T : SlashCommand
    {
        // Suppression Justification
        // ===================================================
        // The initialization of the CommandTypes property is
        //      handled in the constructor for RuntimeOptions.
        _options.CommandTypes!.Add(typeof(T));
        return this;
    }
    /// <summary>
    /// Adds an <see cref="EncapsulatedEventHandler"/> to the DI container.
    /// </summary>
    /// <typeparam name="T">Speicifies the type of <see cref="Command"/> to add.</typeparam>
    /// <returns>The current <see cref="RuntimeOptionsBuilder"/> instance with the specified type added as a hosted service.</returns>
    public RuntimeOptionsBuilder AddEventHandler<T>() where T : EncapsulatedEventHandler
    {
        _ = _services.AddHostedService<T>();
        return this;
    }
    /// <summary>
    /// Adds services to the DI container.
    /// </summary>
    /// <param name="registrationAction">The <see cref="Action{T}"/> that adds services to the DI container on behalf of the consumer.</param>
    /// <returns>The current <see cref="RuntimeOptionsBuilder"/> instance after invoking the registration action.</returns>
    public RuntimeOptionsBuilder AddServices(Action<IServiceCollection> registrationAction)
    {
        registrationAction?.Invoke(_services);
        return this;
    }
    /// <summary>
    /// Adds settings from the current <see cref="IConfiguration"/> to the DI container as a singleton instance.
    /// </summary>
    /// <typeparam name="T">Specifies the type of settings to add.</typeparam>
    /// <param name="key">The key that should be utilized to load the settings.</param>
    /// <returns>The current <see cref="RuntimeOptionsBuilder"/> instance with the specified settings added to the DI container as a singleton instance.</returns>
    public RuntimeOptionsBuilder AddSettings<T>(string key) where T : class, new()
    {
        IConfigurationSection? configurationSection = _configuration.GetSection(key);
        T settings = configurationSection is null
            ? new T()
            : configurationSection.Get<T>() ?? new T();

        _ = _services.AddSingleton(settings);
        return this;
    }
    /// <summary>
    /// Enables a <see cref="Console"/> driven bot for testing purposes.
    /// </summary>
    /// <returns>The current <see cref="RuntimeOptionsBuilder"/> instance with the testing console enabled.</returns>
    public RuntimeOptionsBuilder EnableTesting()
    {
        _options.TestBotEnabled = true;
        return this;
    }
    /// <summary>
    /// Sets the prefix utilized to identify commands in a text-only based chat system.
    /// </summary>
    /// <param name="commandPrefix">The prefix utilized to identify commands in a text-only based chat system.</param>
    /// <returns>The current <see cref="RuntimeOptionsBuilder"/> instance with the command prefix set to the specified value.</returns>
    public RuntimeOptionsBuilder SetCommandPrefix(string commandPrefix)
    {
        _options.CommandPrefix = commandPrefix;
        return this;
    }
    /// <summary>
    /// Sets the separator utilized to identify command arguments in a text-only based chat system.
    /// </summary>
    /// <param name="commandSeparator">The separator utilized to identify command arguments in a text-only based chat system.</param>
    /// <returns>The current <see cref="RuntimeOptionsBuilder"/> instance with the command separator set to the specified value.</returns>
    public RuntimeOptionsBuilder SetCommandSeparator(string commandSeparator)
    {
        _options.CommandSeparator = commandSeparator;
        return this;
    }
    /// <summary>
    /// Builds the <see cref="RuntimeOptions"/> to run bots.
    /// </summary>
    /// <returns>The pre-configured <see cref="RuntimeOptions"/> instance.</returns>
    internal RuntimeOptions Build() =>
        _options;
    /// <summary>
    /// Gets the assemblies from which all <see cref="Chatbot"/> types were registered for MediatR support.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing the registered assemblies.</returns>
    internal IEnumerable<Assembly> GetRegisteredAssemblies() =>
        _chatbotAssemblies.AsReadOnly();
}
