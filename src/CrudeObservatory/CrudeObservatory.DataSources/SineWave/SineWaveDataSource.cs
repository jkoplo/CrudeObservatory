using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.DataSources.Abstractions.Interfaces;
using CrudeObservatory.DataSources.SineWave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.SineWave
{
    internal class SineWaveDataSource : IDataSource
    {
        public SineWaveDataSource(SineWaveDataSourceConfig dataSourceConfig)
        {
            DataSourceConfig = dataSourceConfig ?? throw new ArgumentNullException(nameof(dataSourceConfig));
        }

        public SineWaveDataSourceConfig DataSourceConfig { get; }

        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task<IEnumerable<DataValue>> ReadDataAsync(CancellationToken stoppingToken)
        {
            //I don't need the fractional cycle, but it makes debug easier and my head hurt less
            var totalCycles = DateTimeOffset.Now.ToUnixTimeMilliseconds() / (double)(DataSourceConfig.PeriodSec * 1000);
            var fractionalCycle = totalCycles - Math.Truncate(totalCycles);

            var sineValue = Math.Sin(fractionalCycle * 360);

            var results = new List<DataValue>()
            {
                new DataValue() { Name = DataSourceConfig.Alias, Value = sineValue }
            };

            return Task.FromResult(results.AsEnumerable());
        }

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
