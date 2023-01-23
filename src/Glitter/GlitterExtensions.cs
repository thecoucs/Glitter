using System.Reflection;

using Glitter.Ai;
using Glitter.Commands;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Glitter;

/// <summary>
/// Represents a collection of extension methods for the <see cref="IServiceCollection"/> type which enable adding Glitter services.
/// </summary>
public static class GlitterExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="commandPrefix"></param>
    /// <param name="parameterSeparator"></param>
    /// <returns></returns>
    public static OptionsBuilder<GlitterOptions> EnableTesting(this OptionsBuilder<GlitterOptions> optionsBuilder, string commandPrefix, string parameterSeparator) =>
        optionsBuilder.AddChatbot<ConsoleChatbot<ChatbotOptions>, ChatbotOptions>(chatbotOptionsBuilder =>
            chatbotOptionsBuilder.Configure(options =>
            {
                options.CommandPrefix = commandPrefix;
                options.ParameterSeparator = parameterSeparator;
            })
        );
    /// <summary>
    /// 
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <param name="commandPrefix"></param>
    /// <param name="parameterSeparator"></param>
    /// <returns></returns>
    public static OptionsBuilder<GlitterOptions> EnableTesting<TOptions>(this OptionsBuilder<GlitterOptions> optionsBuilder, string commandPrefix, string parameterSeparator)
        where TOptions : ChatbotOptions =>
        optionsBuilder.AddChatbot<ConsoleChatbot<TOptions>, TOptions>(chatbotOptionsBuilder =>
            chatbotOptionsBuilder.Configure(options =>
            {
                options.CommandPrefix = commandPrefix;
                options.ParameterSeparator = parameterSeparator;
            })
        );
    /// <summary>
    /// Adds a chatbot to the DI container.
    /// </summary>
    /// <typeparam name="T">Specifies the type of the <see cref="Chatbot"/> to add.</typeparam>
    /// <param name="optionsBuilder">The <see cref="OptionsBuilder{TOptions}"/> for configuring Glitter.</param>
    /// <returns>The <see cref="OptionsBuilder{TOptions}"/> for configuring Glitter.</returns>
    public static OptionsBuilder<GlitterOptions> AddChatbot<T, TOptions>(this OptionsBuilder<GlitterOptions> optionsBuilder, Action<OptionsBuilder<TOptions>> configurationAction)
        where T : Chatbot<TOptions> where TOptions : ChatbotOptions
    {
        _ = optionsBuilder.Services.AddHostedService<T>();
        var typeAssembly = Assembly.GetAssembly(typeof(T));
        if (typeAssembly is not null)
            _ = optionsBuilder.Configure(opts => opts.Assemblies.Add(typeAssembly));

        configurationAction?.Invoke(obj: null);
        return optionsBuilder;
    }
    /// <summary>
    /// Adds Glitter services to the DI container.
    /// </summary>
    /// <param name="services">The current service contract for registering services with the DI container.</param>
    /// <param name="buildAction">The <see cref="Action{T}"/> for building options for Glitter.</param>
    /// <returns>The current service contract for registering services with the DI container.</returns>
    public static OptionsBuilder<GlitterOptions> AddGlitter(this IServiceCollection services) =>
        services.AddOptions<GlitterOptions>(name: "Glitter")
                .BindConfiguration(configSectionPath: "Glitter");
    public static OptionsBuilder<GlitterOptions> AddSlashCommand<T>(this OptionsBuilder<GlitterOptions> optionsBuilder)
        where T : SlashCommand =>
        optionsBuilder.Configure(options => options.CommandTypes.Add(typeof(T)));
}
