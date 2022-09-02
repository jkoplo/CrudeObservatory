using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.Ini;

namespace InfluxDbManager
{
    public class InfluxdbCliClient
    {
        private readonly ILogger<InfluxdbCliClient> logger;
        private readonly IConfiguration configuration;

        public InfluxdbCliClient(ILogger<InfluxdbCliClient> logger, IConfiguration configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<InfluxdbConfig?> GetLocalConfig()
        {
            var procPath = Path.GetDirectoryName(Environment.ProcessPath);
            var iniFullPath = GetIniPath(procPath, configuration);

            if (!File.Exists(iniFullPath))
            {
                //TODO: Maybe we need to check requested config against actual?
                logger.LogInformation("No file exists at {InfluxClientIniPath}", iniFullPath);
                return default;
            }

            var clientConfiguration = new ConfigurationBuilder()
                .AddIniFile(iniFullPath, optional: true, reloadOnChange: true)
                .Build();

            var dbConfig = new InfluxdbConfig
            {
                Token = clientConfiguration["default:token"],
                //Bucket = clientConfiguration["default:token"],
                Organization = clientConfiguration["default:org"],
                Url = clientConfiguration["default:url"]
            };

            return dbConfig;
        }

        public async Task InitialConfigureClient(InfluxdbConfig influxdbConfig)
        {
            var procPath = Path.GetDirectoryName(Environment.ProcessPath);
            var currPath = Environment.CurrentDirectory;

            var iniFullPath = GetIniPath(procPath, configuration);

            //Assume an absolute or relative path
            var clientExePath = configuration["Influxdb:ClientExePath"];
            var clientExeFullPath = Path.GetFullPath(clientExePath);
            if (!File.Exists(clientExeFullPath))
                clientExeFullPath = Path.Combine(procPath, clientExePath);

            if (File.Exists(iniFullPath))
            {
                //TODO: Maybe we need to check requested config against actual?
                logger.LogInformation("Influxdb CLI client is already configured");
                return;
            }

            var args = new List<string>
            {
                @"setup",
                @$"--configs-path {iniFullPath}",
                @$"--org {influxdbConfig.Organization}",
                @$"--bucket default",
                @"--retention 0",
                @"--username example-user",
                @"--password ExAmPl3PA55W0rD",
                @$"--token {influxdbConfig.Token}",
                @"--force"
            };

            logger.LogInformation("Attempting to configure Influxdb with CLI client");

            // Calling `ExecuteBufferedAsync()` instead of `ExecuteAsync()`
            // implicitly configures pipes that write to in-memory buffers.
            var result = await Cli.Wrap(clientExeFullPath).WithArguments(String.Join(' ', args)).ExecuteBufferedAsync();

            logger.LogInformation(
                "Influxdb configured by client: Runtime={Runtime}, StandardOut={StandardOut}, StandardError={StandardError}",
                result.RunTime,
                result.StandardOutput,
                result.StandardError
            );
        }

        private string GetIniPath(string basePath, IConfiguration configuration)
        {
            var iniPath = configuration["Influxdb:StoragePath"];
            var iniFullPath = Path.GetFullPath(iniPath);
            if (!Directory.Exists(iniFullPath))
                iniFullPath = Path.Combine(basePath, iniPath);

            return @$"{iniFullPath}\config";
        }
    }
}
