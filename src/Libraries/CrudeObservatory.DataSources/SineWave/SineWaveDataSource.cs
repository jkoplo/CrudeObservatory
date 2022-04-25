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
            double timeInSeconds = DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000;

            double rawValue = 0d;
            double t = DataSourceConfig.Frequency * timeInSeconds + DataSourceConfig.Phase;

            // sin( 2 * pi * t )
            rawValue = (double)Math.Sin(2f * Math.PI * t);

            double invert = DataSourceConfig.Invert ? 1 : -1;

            var scaledValue = (invert * DataSourceConfig.Amplitude * rawValue + DataSourceConfig.Offset);

            var results = new List<IDataValue>()
            {
                new DataValue() { Name = DataSourceConfig.Alias, Value = scaledValue }
            };

            return Task.FromResult(results.AsEnumerable());
        }

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
