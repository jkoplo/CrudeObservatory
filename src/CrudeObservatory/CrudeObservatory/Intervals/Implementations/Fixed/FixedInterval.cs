using CrudeObservatory.Intervals.Abstractions.Interfaces;
using CrudeObservatory.Intervals.Implementations.Fixed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.Intervals.Implementations.Fixed
{
    internal class FixedInterval : IInterval
    {
        private DateTime? intervalExpiration = null;
        public FixedIntervalConfig IntervalConfig { get; }

        public FixedInterval(FixedIntervalConfig intervalConfig)
        {
            IntervalConfig = intervalConfig ?? throw new ArgumentNullException(nameof(intervalConfig));
        }

        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task WaitForIntervalAsync(CancellationToken stoppingToken)
        {
            //Init the expiration if first call
            if (intervalExpiration == null)
                intervalExpiration = DateTime.Now.AddSeconds(IntervalConfig.PeriodSec);

            //Get the remaining time in msec rounded to nearest integer
            var msecTilExpiration = Convert.ToInt32(Math.Round((intervalExpiration - DateTime.Now).Value.TotalMilliseconds));

            if (msecTilExpiration < 0)
                //TODO: Maybe this should error, or have a config option to error
                return Task.CompletedTask;

            return Task.Delay(msecTilExpiration);
        }
    }
}
