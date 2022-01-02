using CrudeObservatory.Abstractions.Models;

namespace CrudeObservatory.Abstractions.Interfaces
{
    public interface IDataTarget
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task WriteAcquisitionConfigAsync(AcquisitionConfig acqConfig, CancellationToken stoppingToken);
        Task WriteDataAsync(IEnumerable<IDataValue> dataValues, CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);
    }
}
