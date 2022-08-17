using InfluxDbManager;

namespace TestInfluxdbServiceWorker
{
    public class TestConfigureInfluxdbWorker : BackgroundService
    {
        private readonly ILogger<TestConfigureInfluxdbWorker> _logger;
        private readonly InfluxdbClient influxdbClient;

        public TestConfigureInfluxdbWorker(ILogger<TestConfigureInfluxdbWorker> logger, InfluxdbClient influxdbClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.influxdbClient = influxdbClient ?? throw new ArgumentNullException(nameof(influxdbClient));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(3000, stoppingToken);
            await influxdbClient.InitialConfigureClient();
        }
    }
}
