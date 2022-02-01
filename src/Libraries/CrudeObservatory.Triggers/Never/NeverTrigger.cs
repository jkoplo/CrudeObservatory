using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Triggers.Delay.Models;

namespace CrudeObservatory.Triggers.Never
{
    public class NeverTrigger : ITrigger
    {
        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;
        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;
        public Task WaitForTriggerAsync(CancellationToken stoppingToken) => Task.Delay(Timeout.Infinite, stoppingToken);

    }
}
