using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.DataTargets.Abstractions.Interfaces;
using CrudeObservatory.DataTargets.Implementations.CSV.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudeObservatory.DataTargets.Implementations.CSV
{
    internal class CsvDataTarget : IDataTarget
    {
        public CsvDataTarget(CsvDataTargetConfig dataTargetConfig)
        {
            DataTargetConfig = dataTargetConfig ?? throw new ArgumentNullException(nameof(dataTargetConfig));
        }

        public CsvDataTargetConfig DataTargetConfig { get; }

        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task WriteAcquisitionConfigAsync(AcquisitionConfig acqConfig) => Task.CompletedTask;

        public async Task WriteDataAsync(IEnumerable<DataValue> dataValues, CancellationToken stoppingToken)
        {
            var records = dataValues.Select(x => x.Value);

            // Append to the file.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(DataTargetConfig.FilePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                await csv.WriteRecordsAsync(records);
            }
        }
    }
}
