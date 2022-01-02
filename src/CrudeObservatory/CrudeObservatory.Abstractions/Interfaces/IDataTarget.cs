using CrudeObservatory.Acquisition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataTargets.Abstractions.Interfaces
{
    internal interface IDataTarget
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task WriteAcquisitionConfigAsync(AcquisitionConfig acqConfig, CancellationToken stoppingToken);
        Task WriteDataAsync(IEnumerable<DataValue> dataValues, CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);
    }
}
