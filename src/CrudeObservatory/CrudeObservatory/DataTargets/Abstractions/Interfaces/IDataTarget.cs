using CrudeObservatory.DataTargets.Abstractions.Models;
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
        Task WriteDataAsync(IEnumerable<DataValue> dataValues, CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);

    }
}
