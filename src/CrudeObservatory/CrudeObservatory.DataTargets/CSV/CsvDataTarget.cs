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
        private bool firstDataWrite;
        private readonly string csvPath;

        public CsvDataTarget(CsvDataTargetConfig dataTargetConfig)
        {
            DataTargetConfig = dataTargetConfig ?? throw new ArgumentNullException(nameof(dataTargetConfig));
            firstDataWrite = true;
            csvPath = Path.GetFullPath(DataTargetConfig.FilePath);
        }

        public CsvDataTargetConfig DataTargetConfig { get; }

        public Task InitializeAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public Task ShutdownAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public async Task WriteAcquisitionConfigAsync(AcquisitionConfig acqConfig, CancellationToken stoppingToken)
        {
            //var records = new List<AcquisitionConfig>()
            //{
            //    acqConfig
            //};

            //// Append to the file.
            //using (var stream = File.Open(csvPath, FileMode.Create))
            //using (var writer = new StreamWriter(stream))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //{
            //    await csv.WriteRecordsAsync(records, stoppingToken);
            //}

        }

        public async Task WriteDataAsync(IEnumerable<DataValue> dataValues, CancellationToken stoppingToken)
        {
            if (firstDataWrite)
            {
                if (File.Exists(csvPath))
                    File.Delete(csvPath);

                var names = dataValues.Select(x => x.Name as object).ToList();
                await WriteCsvRow(names, stoppingToken);
                firstDataWrite = false;
            }

            var values = dataValues.Select(x => x.Value).ToList();
            await WriteCsvRow(values, stoppingToken);
        }

        private async Task WriteCsvRow(List<object> values, CancellationToken stoppingToken)
        {
            var csvRecord = new CsvRecord()
            {
                DataValues = values,
            };

            var records = new List<CsvRecord>()
            {
                csvRecord
            };


            // Append to the file.
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(csvPath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                await csv.WriteRecordsAsync(records, stoppingToken);
            }
        }
    }
}
