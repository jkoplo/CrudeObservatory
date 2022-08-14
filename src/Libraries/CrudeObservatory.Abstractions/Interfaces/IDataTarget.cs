using CrudeObservatory.Abstractions.Models;

namespace CrudeObservatory.Abstractions.Interfaces
{
    public interface IDataTarget
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task WriteAcquisitionConfigAsync(AcquisitionConfig acqConfig, CancellationToken stoppingToken);
        Task WriteDataAsync(
            IIntervalOutput intervalData,
            IEnumerable<IDataValue> sourceData,
            CancellationToken stoppingToken
        );
        Task ShutdownAsync(CancellationToken stoppingToken);
    }
}
