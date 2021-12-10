using CrudeObservatory.DataSources.Abstractions.Interfaces;
using CrudeObservatory.DataSources.Implementations.SineWave.Models;
using CrudeObservatory.DataTargets.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataSources.Implementations.SineWave
{
    internal class SineWaveDataSource : IDataSource
    {
        public SineWaveDataSource(SineWaveDataSourceConfig dataSourceConfig)
        {
            DataSourceConfig = dataSourceConfig ?? throw new ArgumentNullException(nameof(dataSourceConfig));
        }

        public SineWaveDataSourceConfig DataSourceConfig { get; }

        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task<DataValue> ReadDataAsync(CancellationToken stoppingToken)
        {
            //I don't need the fractional cycle, but it makes debug easier and my head hurt less
            var totalCycles = (double)DateTimeOffset.Now.ToUnixTimeMilliseconds() / (double)(DataSourceConfig.PeriodSec * 1000);
            var fractionalCycle = totalCycles - Math.Truncate(totalCycles);

            var sineValue = Math.Sin(fractionalCycle * 360);

            return Task.FromResult(new DataValue() { Name = DataSourceConfig.Alias, Value = sineValue });
        }

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
