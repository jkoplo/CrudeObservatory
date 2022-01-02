using CrudeObservatory.Acquisition.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.Abstractions.Interfaces
{
    internal interface IDataSource
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task<IEnumerable<DataValue>> ReadDataAsync(CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);

    }
}
