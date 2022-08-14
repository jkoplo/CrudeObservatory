namespace CrudeObservatory.Abstractions.Interfaces
{
    public interface IDataSource
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task<IEnumerable<IDataValue>> ReadDataAsync(CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);
    }
}
