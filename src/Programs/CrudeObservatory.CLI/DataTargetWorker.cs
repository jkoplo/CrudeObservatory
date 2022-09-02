using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.DataTargets.InfluxDB;
using CrudeObservatory.DataTargets.InfluxDB.Models;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using InfluxDbManager;
using System.Security.Cryptography;
using System.Threading.Channels;

namespace CrudeObservatory.CLI
{
    public class DataTargetWorker : BackgroundService
    {
        private const string DEFAULT_BUCKET = "crude_observatory";
        private readonly ILogger<DataTargetWorker> logger;
        private readonly InfluxdbCliClient influxdbClient;
        private readonly ChannelReader<Measurement> channel;

        public DataTargetWorker(
            ILogger<DataTargetWorker> logger,
            InfluxdbCliClient influxdbClient,
            ChannelReader<Measurement> channel
        )
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.influxdbClient = influxdbClient ?? throw new ArgumentNullException(nameof(influxdbClient));
            this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
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
                    //Default Influx tokens are 512 bit random
                    Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                    Organization = "TestOrg",
                    Url = "http://localhost:8086"
                };

                await influxdbClient.InitialConfigureClient(dbConfig);
            }

            var client = InfluxDBClientFactory.Create(dbConfig.Url, dbConfig.Token);

            //Get the organization by name
            var org = (
                await client
                    .GetOrganizationsApi()
                    .FindOrganizationsAsync(org: dbConfig.Organization, cancellationToken: stoppingToken)
            ).Single();

            var orgBuckets = await client.GetBucketsApi().FindBucketsByOrganizationAsync(org, stoppingToken);

            //If no default bucket, create it
            if (!orgBuckets.Select(x => x.Name).Contains(DEFAULT_BUCKET))
            {
                //Create default bucket

                var retention = new BucketRetentionRules(BucketRetentionRules.TypeEnum.Expire, 0);

                var bucket = await client
                    .GetBucketsApi()
                    .CreateBucketAsync(DEFAULT_BUCKET, retention, org, stoppingToken);
            }

            InfluxDBDataTargetConfig dataTargetConfig =
                new()
                {
                    Url = dbConfig.Url,
                    Organization = dbConfig.Organization,
                    Bucket = DEFAULT_BUCKET,
                    Token = dbConfig.Token,
                    Measurement = "CLI_Test"
                };

            InfluxDBDataTarget influxTarget = new(dataTargetConfig);

            await influxTarget.InitializeAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //We could also read all
                    var message = await channel.ReadAsync(stoppingToken);
                    await influxTarget.WriteDataAsync(message.IntervalOutput, message.DataValues, stoppingToken);
                }
                catch (OperationCanceledException ex)
                {
                    logger.LogWarning($"DataTargetWorker > forced stop");
                }
            }
        }
    }
}
