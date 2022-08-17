using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CliWrap;
using CliWrap.EventStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace InfluxDbManager
{
    public class InfluxdbWorker : BackgroundService
    {
        private readonly ILogger<InfluxdbWorker> logger;
        private readonly IConfiguration configuration;

        public InfluxdbWorker(ILogger<InfluxdbWorker> logger, IConfiguration configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var dbExePath = configuration["Influxdb:DbExePath"];
            var storagePath = configuration["Influxdb:StoragePath"];

            var procPath = Path.GetDirectoryName(Environment.ProcessPath);
            var currPath = Environment.CurrentDirectory;

            //Assume an absolute or relative path
            var dbExeFullPath = Path.GetFullPath(dbExePath);
            var storageFullPath = Path.GetFullPath(storagePath);

            if (!Directory.Exists(dbExeFullPath))
                dbExeFullPath = Path.Combine(procPath, dbExePath);

            if (!Directory.Exists(storageFullPath))
                storageFullPath = Path.Combine(procPath, storagePath);

            var args = new List<string>
            {
                @"--reporting-disabled",
                $@"--bolt-path={storageFullPath}\influxd.bolt",
                $@"--engine-path={storageFullPath}\engine",
                @"--http-bind-address=localhost:8086" //Force local and 8086 for now
            };

            var cmd = Cli.Wrap(dbExeFullPath).WithArguments(args);

            await foreach (var cmdEvent in cmd.ListenAsync(stoppingToken))
            {
                switch (cmdEvent)
                {
                    case StartedCommandEvent started:
                        Console.WriteLine($"Process started; ID: {started.ProcessId}");
                        break;
                    case StandardOutputCommandEvent stdOut:
                        Console.WriteLine($"Out> {stdOut.Text}");
                        break;
                    case StandardErrorCommandEvent stdErr:
                        Console.WriteLine($"Err> {stdErr.Text}");
                        break;
                    case ExitedCommandEvent exited:
                        Console.WriteLine($"Process exited; Code: {exited.ExitCode}");
                        break;
                }
            }
        }
    }
}
