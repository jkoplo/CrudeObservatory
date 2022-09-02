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
    public class InfluxdbWorker : BackgroundService
    {
        private readonly ILogger<InfluxdbWorker> logger;
        private readonly InfluxdbDaemon influxdbDaemon;
        private readonly IConfiguration configuration;

        public InfluxdbWorker(ILogger<InfluxdbWorker> logger, InfluxdbDaemon influxdbDaemon)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.influxdbDaemon = influxdbDaemon ?? throw new ArgumentNullException(nameof(influxdbDaemon));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await influxdbDaemon.ExecuteAsync(stoppingToken);
        }
    }
}
