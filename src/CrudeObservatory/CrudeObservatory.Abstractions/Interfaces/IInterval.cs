namespace CrudeObservatory.Abstractions.Interfaces
{
    public interface IInterval
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task<IEnumerable<IDataValue>> WaitForIntervalAsync(CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);
    }
}
