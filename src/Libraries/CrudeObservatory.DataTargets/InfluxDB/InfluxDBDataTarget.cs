using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.DataTargets.InfluxDB.Models;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataTargets.InfluxDB
{
    public class InfluxDBDataTarget : IDataTarget
    {
        private InfluxDBClient client;

        public InfluxDBDataTarget(InfluxDBDataTargetConfig dataTargetConfig)
        {
            DataTargetConfig = dataTargetConfig ?? throw new ArgumentNullException(nameof(dataTargetConfig));
        }

        public InfluxDBDataTargetConfig DataTargetConfig { get; }

        public async Task InitializeAsync(CancellationToken stoppingToken)
        {
            client = InfluxDBClientFactory.Create(DataTargetConfig.Url, DataTargetConfig.Token);
        }

        public async Task ShutdownAsync(CancellationToken stoppingToken)
        {
            client.Dispose();
        }

        public Task WriteAcquisitionConfigAsync(AcquisitionConfig acqConfig, CancellationToken stoppingToken) =>
            Task.CompletedTask;

        public async Task WriteDataAsync(
            IIntervalOutput intervalData,
            IEnumerable<IDataValue> sourceData,
            CancellationToken stoppingToken
        )
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(intervalData.NominalTime);

            var points = sourceData
                .Select(
                    x =>
                        PointData.Builder
                            .Measurement(DataTargetConfig.Measurement)
                            //.Tag("host", "host2")
                            .SetFieldByObjectType(x.Name, x.Value)
                            .Timestamp(dateTimeOffset.UtcDateTime, WritePrecision.Ms)
                            .ToPointData()
                )
                .ToList();

            using (var writeApi = client.GetWriteApi())
            {
                writeApi.WritePoints(points, DataTargetConfig.Bucket, DataTargetConfig.Organization);
            }
        }
    }
}
