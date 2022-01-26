namespace CrudeObservatory.Abstractions.Interfaces
{
    public interface IInterval
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task<IIntervalOutput> WaitForIntervalAsync(CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);
    }
}
