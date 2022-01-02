using CrudeObservatory.Acquisition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Intervals.Abstractions.Interfaces
{
    internal interface IInterval
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task<IEnumerable<DataValue>> WaitForIntervalAsync(CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);
    }
}
