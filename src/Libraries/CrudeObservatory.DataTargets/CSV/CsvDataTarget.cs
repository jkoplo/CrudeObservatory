using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.DataTargets.CSV.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CrudeObservatory.DataTargets.CSV
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

        public async Task WriteDataAsync(IIntervalOutput intervalData, IEnumerable<IDataValue> sourceData, CancellationToken stoppingToken)
        {
            if (firstDataWrite)
            {
                if (File.Exists(csvPath))
                    File.Delete(csvPath);

                var names = sourceData.Select(x => x.Name as object).ToList();
                names.Insert(0, "Nominal Time");
                await WriteCsvRow(names, stoppingToken);
                firstDataWrite = false;
            }

            var values = sourceData.Select(x => x.Value).ToList();
            //If value is null return -1
            values.Insert(0, intervalData.NominalTime);
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
