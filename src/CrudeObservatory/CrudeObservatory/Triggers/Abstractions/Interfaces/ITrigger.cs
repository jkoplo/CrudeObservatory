using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Triggers.Abstractions.Interfaces
{
    internal interface ITrigger
    {
        Task InitializeAsync(CancellationToken stoppingToken);
        Task WaitForTriggerAsync(CancellationToken stoppingToken);
        Task ShutdownAsync(CancellationToken stoppingToken);
    }
}
