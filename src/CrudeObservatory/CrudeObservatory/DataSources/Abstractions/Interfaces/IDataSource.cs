using CrudeObservatory.DataTargets.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.Abstractions.Interfaces
{
    internal interface IDataSource
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task<DataValue> ReadDataAsync(CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);

    }
}
