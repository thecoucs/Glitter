using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Glitter;

/// <summary>
/// Represents a basic event handler.
/// </summary>
public abstract class EncapsulatedEventHandler : IHostedService
{
    private readonly TimeSpan _pingInterval;
    /// <summary>
    /// The logger for the service the event is related to.
    /// </summary>
    public ILogger Logger { get; private set; }
    /// <summary>
    /// Creates a new <see cref="EncapsulatedEventHandler"/> instance.
    /// </summary>
    /// <param name="logger">The logger for the service the event is related to.</param>
    public EncapsulatedEventHandler(ILogger logger) =>
        Logger = logger;
    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Subscribe();
        await Task.CompletedTask;
    }
    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Unsubscribe();
        await Task.CompletedTask;
    }
    /// <summary>
    /// Subscribes to events.
    /// </summary>
    /// <returns>A <see cref="Task"/> describing the state of the operation.</returns>
    protected abstract void Subscribe();
    /// <summary>
    /// Unsubscribes from events.
    /// </summary>
    /// <returns>A <see cref="Task"/> describing the state of the operation.</returns>
    protected abstract void Unsubscribe();
}