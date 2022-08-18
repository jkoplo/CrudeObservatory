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

            //var dataTargetConfig = new InfluxDBDataTargetConfig
            //{
            //    Token = dbConfig.Token,
            //    Bucket = dbConfig.Bucket,
            //    Organization = dbConfig.Organization,
            //    Url = dbConfig.Url,
            //    Measurement = "TestMeasurement"
            //};

            var client = InfluxDBClientFactory.Create(dbConfig.Url, dbConfig.Token);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(500);
                DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

                var points = new List<PointData>
                {
                    PointData.Builder
                        .Measurement("TestMeasurement")
                        //.Tag("host", "host2")
                        .Field("First", 12345)
                        .Timestamp(dateTimeOffset.UtcDateTime, WritePrecision.Ns)
                        .ToPointData(),
                    PointData.Builder
                        .Measurement("TestMeasurement")
                        //.Tag("host", "host2")
                        .Field("Second", 54321)
                        .Timestamp(dateTimeOffset.UtcDateTime, WritePrecision.Ns)
                        .ToPointData(),
                };

                using (var writeApi = client.GetWriteApi())
                {
                    writeApi.WritePoints(points, dbConfig.Bucket, dbConfig.Organization);
                }
            }
        }
    }
}
