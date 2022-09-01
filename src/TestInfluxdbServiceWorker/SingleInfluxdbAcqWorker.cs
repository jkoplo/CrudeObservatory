using CrudeObservatory.DataTargets.InfluxDB;
using CrudeObservatory.DataTargets.InfluxDB.Models;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using InfluxDbManager;

namespace TestInfluxdbServiceWorker
{
    public class SingleInfluxdbAcqWorker : BackgroundService
    {
        private const string DEFAULT_BUCKET = "crude_observatory";
        private readonly ILogger<SingleInfluxdbAcqWorker> _logger;
        private readonly InfluxdbClient influxdbClient;

        public SingleInfluxdbAcqWorker(ILogger<SingleInfluxdbAcqWorker> logger, InfluxdbClient influxdbClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.influxdbClient = influxdbClient ?? throw new ArgumentNullException(nameof(influxdbClient));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Wait for the other worker to bring the process up
            await Task.Delay(3000, stoppingToken);

            //We need to make sure the Influx instance is configured
            //and that we have proper values for use with our client
            // 1) Local CLI config present - read from disk and use
            // 2) No local CLI config present - configure Influx
            // 3) TODO: Influx is controlled outside of this application

            var dbConfig = await influxdbClient.GetLocalConfig();

            //No config present - configure the client
            if (dbConfig == null)
            {
                dbConfig = new InfluxdbConfig
                {
                    Token = Guid.NewGuid().ToString(),
                    //Bucket = "TEST",
                    Organization = "TestOrg",
                    Url = "http://localhost:8086"
                };

                await influxdbClient.InitialConfigureClient(dbConfig);
            }

            var client = InfluxDBClientFactory.Create(dbConfig.Url, dbConfig.Token);

            var org = (
                await client
                    .GetOrganizationsApi()
                    .FindOrganizationsAsync(org: dbConfig.Organization, cancellationToken: stoppingToken)
            ).Single();

            var orgBuckets = await client.GetBucketsApi().FindBucketsByOrganizationAsync(org, stoppingToken);

            if (!orgBuckets.Select(x => x.Name).Contains(DEFAULT_BUCKET))
            {
                //Create default bucket

                var retention = new BucketRetentionRules(BucketRetentionRules.TypeEnum.Expire, 0);

                var bucket = await client
                    .GetBucketsApi()
                    .CreateBucketAsync(DEFAULT_BUCKET, retention, org, stoppingToken);
            }

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

                writeApi.WritePoints(points, DEFAULT_BUCKET, dbConfig.Organization);
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
