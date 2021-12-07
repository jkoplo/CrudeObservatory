namespace CrudeObservatory
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Pull in config - might be at the Program.cs DI level
            //Build out classes - might be at the Program.cs DI level

            //Initiate connections? Could be DataSources (PLC) or DataTargets (DB)
            //Wait for start trigger

            while (!stoppingToken.IsCancellationRequested)
            {
                //Wait for interval OR end trigger
                //Get data from source(s)
                //Write data to target(s)
                //Repeat @ Wait for interval OR end trigger

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}