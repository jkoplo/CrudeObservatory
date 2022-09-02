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
using System.Diagnostics;

namespace InfluxDbManager
{
    public class InfluxdbDaemon
    {
        private readonly ILogger<InfluxdbDaemon> logger;
        private readonly IConfiguration configuration;

        public InfluxdbDaemon(ILogger<InfluxdbDaemon> logger, IConfiguration configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var processes = Process.GetProcessesByName("influxd");

            foreach (var process in processes)
                logger.LogWarning("Found a running influxd: PID={pid}", process.Id);

            var dbExePath = configuration["Influxdb:DbExePath"];
            var storagePath = configuration["Influxdb:StoragePath"];

            var procPath = Path.GetDirectoryName(Environment.ProcessPath);
            var currPath = Environment.CurrentDirectory;

            //Assume an absolute or relative path
            var dbExeFullPath = Path.GetFullPath(dbExePath);
            var storageFullPath = Path.GetFullPath(storagePath);

            if (!File.Exists(dbExeFullPath))
                dbExeFullPath = Path.Combine(procPath, dbExePath);

            if (!Directory.Exists(storageFullPath))
            {
                storageFullPath = Path.Combine(procPath, storagePath);
                Directory.CreateDirectory(storageFullPath);
            }

            var pidFullPath = $@"{storageFullPath}\pid";

            //Check if the previously started influxd is still running
            if (File.Exists(pidFullPath))
            {
                var pidString = await File.ReadAllTextAsync(pidFullPath);
                var pid = int.Parse(pidString);
                var processToKill = Process.GetProcesses().SingleOrDefault(x => x.Id == pid);

                //Verify it's actually an influxd
                if (processToKill?.ProcessName == "influxd" && processToKill != null)
                    processToKill.Kill();
            }

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
                        logger.LogInformation($"Influxdb started: ID={started.ProcessId}");

                        await File.WriteAllTextAsync(pidFullPath, started.ProcessId.ToString(), stoppingToken);

                        //var process = Process.GetProcessById(started.ProcessId);
                        //logger.LogInformation($"Influxdb started: Process={@process}", process);
                        //process.Kill();

                        break;
                    case StandardOutputCommandEvent stdOut:
                        logger.LogInformation($"Influxdb: {stdOut.Text}");
                        break;
                    case StandardErrorCommandEvent stdErr:
                        logger.LogError($"Influxdb: {stdErr.Text}");
                        break;
                    case ExitedCommandEvent exited:
                        logger.LogInformation($"Process exited; Code: {exited.ExitCode}");
                        break;
                }
            }
        }
    }
}
