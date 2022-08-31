using CrudeObservatory.DataTargets.InfluxDB;
using CrudeObservatory.DataTargets.InfluxDB.Models;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using InfluxDbManager;

namespace TestInfluxdbServiceWorker
{
    public class TestConfigureInfluxdbWorker : BackgroundService
    {
        private readonly ILogger<TestConfigureInfluxdbWorker> _logger;
        private readonly InfluxdbClient influxdbClient;

        public TestConfigureInfluxdbWorker(ILogger<TestConfigureInfluxdbWorker> logger, InfluxdbClient influxdbClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.influxdbClient = influxdbClient ?? throw new ArgumentNullException(nameof(influxdbClient));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var dbConfig = new InfluxdbConfig
            {
                Token = Guid.NewGuid().ToString(),
                Bucket = "TEST",
                Organization = "TestOrg",
                Url = "http://localhost:8086"
            };

            await Task.Delay(3000, stoppingToken);
            await influxdbClient.InitialConfigureClient(dbConfig);

            var client = InfluxDBClientFactory.Create(dbConfig.Url, dbConfig.Token);
            using var writeApi = client.GetWriteApi();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(100);
                DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

                var sineWave = SineWave();

                var points = new List<PointData>
                {
                    PointData.Builder
                        .Measurement("TestMeasurement")
                        //.Tag("host", "host2")
                        .Field("First", sineWave)
                        .Timestamp(dateTimeOffset.UtcDateTime, WritePrecision.Ns)
                        .ToPointData(),
                    PointData.Builder
                        .Measurement("TestMeasurement")
                        //.Tag("host", "host2")
                        .Field("Second", sineWave * -1)
                        .Timestamp(dateTimeOffset.UtcDateTime, WritePrecision.Ns)
                        .ToPointData(),
                };

                writeApi.WritePoints(points, dbConfig.Bucket, dbConfig.Organization);
            }
        }

        private double SineWave()
        {
            //https://www.codeproject.com/Articles/30180/Simple-Signal-Generator

            double timeInSeconds = DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000;

            double rawValue = 0d;
            double t = .01 * timeInSeconds;

            // sin( 2 * pi * t )
            rawValue = (double)Math.Sin(2f * Math.PI * t);

            var scaledValue = (100 * rawValue);
            return scaledValue;
        }
    }
}
