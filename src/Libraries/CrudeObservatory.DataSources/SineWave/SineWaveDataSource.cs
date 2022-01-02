using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.DataSources.SineWave.Models;

namespace CrudeObservatory.DataSources.SineWave
{
    public class SineWaveDataSource : IDataSource
    {
        public SineWaveDataSource(SineWaveDataSourceConfig dataSourceConfig)
        {
            DataSourceConfig = dataSourceConfig ?? throw new ArgumentNullException(nameof(dataSourceConfig));
        }

        public SineWaveDataSourceConfig DataSourceConfig { get; }

        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task<IEnumerable<IDataValue>> ReadDataAsync(CancellationToken stoppingToken)
        {
            //I don't need the fractional cycle, but it makes debug easier and my head hurt less
            var totalCycles = DateTimeOffset.Now.ToUnixTimeMilliseconds() / (double)(DataSourceConfig.PeriodSec * 1000);
            var fractionalCycle = totalCycles - Math.Truncate(totalCycles);

            var sineValue = Math.Sin(fractionalCycle * 360);

            var results = new List<IDataValue>()
            {
                new DataValue() { Name = DataSourceConfig.Alias, Value = sineValue }
            };

            return Task.FromResult(results.AsEnumerable());
        }

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
