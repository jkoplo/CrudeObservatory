namespace CrudeObservatory.Abstractions.Interfaces
{
    public interface ITrigger
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task WaitForTriggerAsync(CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);
    }
}
