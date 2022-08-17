using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InfluxDbManager
{
    public class InfluxdbClient
    {
        private readonly ILogger<InfluxdbClient> logger;
        private readonly IConfiguration configuration;

        public InfluxdbClient(ILogger<InfluxdbClient> logger, IConfiguration configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task InitialConfigureClient()
        {
            var clientExePath = configuration["Influxdb:ClientExePath"];
            var storagePath = configuration["Influxdb:StoragePath"];

            var procPath = Path.GetDirectoryName(Environment.ProcessPath);
            var currPath = Environment.CurrentDirectory;

            //Assume an absolute or relative path
            var clientExeFullPath = Path.GetFullPath(clientExePath);
            var storageFullPath = Path.GetFullPath(storagePath);

            if (!File.Exists(clientExeFullPath))
                clientExeFullPath = Path.Combine(procPath, clientExePath);

            if (!Directory.Exists(storageFullPath))
                storageFullPath = Path.Combine(procPath, storagePath);

            var args = new List<string>
            {
                @"setup",
                @$"--configs-path {storageFullPath}\config",
                @"--org example-org",
                @"--bucket example-bucket",
                @"--username example-user",
                @"--password ExAmPl3PA55W0rD",
                @"--retention 0",
                @"--force"
            };

            logger.LogInformation("Attempting to configure Influxdb with CLI client");

            // Calling `ExecuteBufferedAsync()` instead of `ExecuteAsync()`
            // implicitly configures pipes that write to in-memory buffers.
            var result = await Cli.Wrap(clientExeFullPath)
                .WithArguments(String.Join(' ', args))
                //.WithArguments(
                //    args =>
                //        args.Add("setup")
                //            .Add(@$"--configs-path {storageFullPath}\config")
                //            .Add(@"--org example-org")
                //            .Add(@"--bucket example-bucket")
                //)
                //.WithArguments(@$"--configs-path {storageFullPath}\config")
                //.WithWorkingDirectory("work/dir/path")
                .ExecuteBufferedAsync();

            logger.LogInformation(
                "Influxdb configured by client: Runtime={Runtime}, StandardOut={StandardOut}, StandardError={StandardError}",
                result.RunTime,
                result.StandardOutput,
                result.StandardError
            );
        }
    }
}
