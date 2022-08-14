using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Triggers.Delay.Models;

namespace CrudeObservatory.Triggers.Delay
{
    public class DelayTrigger : ITrigger
    {
        public DelayTrigger(DelayTriggerConfig triggerConfig)
        {
            TriggerConfig = triggerConfig ?? throw new ArgumentNullException(nameof(triggerConfig));
        }

        public DelayTriggerConfig TriggerConfig { get; }

        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task WaitForTriggerAsync(CancellationToken stoppingToken)
        {
            //Convert seconds to nearest msec
            var msecDelay = Convert.ToInt32(Math.Round(TriggerConfig.DelaySeconds * 1000));
            return Task.Delay(msecDelay, stoppingToken);
        }
    }
}
