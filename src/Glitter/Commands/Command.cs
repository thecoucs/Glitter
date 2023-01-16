using Mauve.Extensibility;
using Mauve.Math;

using Microsoft.Extensions.Logging;

namespace Glitter.Commands;

public abstract class Command
{
    private readonly string _id;
    protected ILogger Logger { get; private set; }
    public string Key { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public Command(string key, string displayName, string description, ILogger logger)
    {
        _id = Guid.NewGuid().GetHashCode(NumericBase.Hexadecimal);
        Key = key;
        Logger = logger;
        DisplayName = displayName;
        Description = description;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be utilized during execution to signal cancellation.</param>
    /// <returns></returns>
    public async Task<CommandResponse?> Execute(CancellationToken cancellationToken)
    {
        bool encounteredErrors = false;
        CommandResponse? response = null;
        Logger.LogDebug($"Executing command '{DisplayName}'...");
        try
        {
            response = await Work(cancellationToken);
        } catch (Exception e)
        {
            Logger.LogError($"An unexpected error occurred during execution. {e.Message}");
        } finally
        {
            // Set the event type for the completion message.
            LogLevel logLevel = encounteredErrors
                ? LogLevel.Error
                : LogLevel.Information;

            // Log the completion message.
            Logger.Log(logLevel, "Execution complete.");
        }

        return await Task.FromResult(response);
    }
    protected abstract Task<CommandResponse> Work(CancellationToken cancellationToken);
}