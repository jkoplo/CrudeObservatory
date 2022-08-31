using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Intervals.Fixed.Models;

namespace CrudeObservatory.Intervals.Fixed
{
    internal class PeriodicInterval : IInterval
    {
        private long? intervalExpiration = null;
        public PeriodicIntervalConfig IntervalConfig { get; }

        public PeriodicInterval(PeriodicIntervalConfig intervalConfig)
        {
            IntervalConfig = intervalConfig ?? throw new ArgumentNullException(nameof(intervalConfig));
        }

        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public async Task<IIntervalOutput> WaitForIntervalAsync(CancellationToken stoppingToken)
        {
            //Init the expiration if first call
            if (intervalExpiration == null)
                intervalExpiration =
                    DateTimeOffset.Now.ToUnixTimeMilliseconds() + Convert.ToInt64(IntervalConfig.PeriodSec * 1000);

            //Get the remaining time in msec rounded to nearest integer
            var msecTilExpiration = Convert.ToInt32(intervalExpiration - DateTimeOffset.Now.ToUnixTimeMilliseconds());

            var intervalOutput = new IntervalOutput() { NominalTime = intervalExpiration ?? long.MinValue };

            intervalExpiration += Convert.ToInt64(IntervalConfig.PeriodSec * 1000);

            if (msecTilExpiration < 0)
                //TODO: Maybe this should error, or have a config option to error
                return intervalOutput;

            await Task.Delay(msecTilExpiration);
            return intervalOutput;
        }
    }
}
