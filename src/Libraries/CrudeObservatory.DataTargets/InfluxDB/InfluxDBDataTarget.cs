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
        private readonly InfluxDBDataTargetConfig config;
        private InfluxDBClient client;

        public InfluxDBDataTarget(InfluxDBDataTargetConfig dataTargetConfig)
        {
            DataTargetConfig = dataTargetConfig ?? throw new ArgumentNullException(nameof(dataTargetConfig));
        }

        public InfluxDBDataTargetConfig DataTargetConfig { get; }

        public Task InitializeAsync(CancellationToken stoppingToken)
        {
            client = InfluxDBClientFactory.Create(DataTargetConfig.Url, DataTargetConfig.Token);
            return Task.CompletedTask;
        }

        public Task ShutdownAsync(CancellationToken stoppingToken)
        {
            client.Dispose();
            return Task.CompletedTask;
        }

        public Task WriteAcquisitionConfigAsync(AcquisitionConfig acqConfig, CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        public Task WriteDataAsync(IEnumerable<IDataValue> dataValues, CancellationToken stoppingToken)
        {
            //TODO: Need to either extract "Nominal Time" or pass timestamp on datavalues or pass an output from interval explicitly

            var values = dataValues.Select(x => PointData
                                                  .Measurement(DataTargetConfig.Measurement)
                                                  //.Tag("host", "host2")
                                                  .SetFieldByObjectType(x.Name, x.Value)
                                                  .Timestamp(DateTime.UtcNow, WritePrecision.Ns))
                                                .ToList();


            var point = PointData
              .Measurement("mem")
              .Tag("host", "host2")
              .Field("used_percent", 38.43234543)
              .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            using (var writeApi = client.GetWriteApi())
            {
                writeApi.WritePoint(bucket, org, point);
            }

            throw new NotImplementedException();
        }


    }
}