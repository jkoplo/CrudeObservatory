using CrudeObservatory.Acquisition.Models;
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
        private long? intervalExpiration = null;
        public FixedIntervalConfig IntervalConfig { get; }

        public FixedInterval(FixedIntervalConfig intervalConfig)
        {
            IntervalConfig = intervalConfig ?? throw new ArgumentNullException(nameof(intervalConfig));
        }

        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public async Task<IEnumerable<DataValue>> WaitForIntervalAsync(CancellationToken stoppingToken)
        {

            //Init the expiration if first call
            if (intervalExpiration == null)
                intervalExpiration = DateTimeOffset.Now.ToUnixTimeMilliseconds() + Convert.ToInt64(IntervalConfig.PeriodSec * 1000);

            //Get the remaining time in msec rounded to nearest integer
            var msecTilExpiration = Convert.ToInt32(intervalExpiration - DateTimeOffset.Now.ToUnixTimeMilliseconds());

            var intervalValues = new List<DataValue>()
            {
                new DataValue()
                {
                    Name="Nominal Time", 
                    Value= intervalExpiration
                }
            };

            intervalExpiration = intervalExpiration + Convert.ToInt64(IntervalConfig.PeriodSec * 1000);

            if (msecTilExpiration < 0)
                //TODO: Maybe this should error, or have a config option to error
                return intervalValues;

            await Task.Delay(msecTilExpiration);
            return intervalValues;
        }
    }
}
