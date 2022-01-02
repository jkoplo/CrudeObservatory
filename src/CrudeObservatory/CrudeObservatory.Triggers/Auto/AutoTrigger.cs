using CrudeObservatory.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Triggers.Auto
{
    internal class AutoTrigger : ITrigger
    {
        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task WaitForTriggerAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
