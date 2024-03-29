﻿using CrudeObservatory.Abstractions.Interfaces;
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

        public Task WriteAcquisitionConfigAsync(AcquisitionConfig acqConfig, CancellationToken stoppingToken) =>
            Task.CompletedTask;

        public Task WriteDataAsync(
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
                            .Timestamp(dateTimeOffset.UtcDateTime, WritePrecision.Ns)
                            .ToPointData()
                )
                .ToList();

            using (var writeApi = client.GetWriteApi())
            {
                writeApi.WritePoints(DataTargetConfig.Bucket, DataTargetConfig.Organization, points);
            }
            return Task.CompletedTask;
        }
    }
}
