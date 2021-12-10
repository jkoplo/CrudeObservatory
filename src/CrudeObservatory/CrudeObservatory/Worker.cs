using CrudeObservatory.Acquisition.Models;

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
            var acqConfig = new AcquisitionConfig();

            //Build out classes - might be at the Program.cs DI level

            AcquisitionSet acq = new AcquisitionSet();




            //Initiate connections? Could be DataSources (PLC) or DataTargets (DB) or Intervals(?)
            //Build a list of tasks to init so we can execute them in parallel
            var initTasks = new List<Task>()
            {
                acq.StartTrigger.InitializeAsync(stoppingToken),
                acq.EndTrigger.InitializeAsync(stoppingToken),
                acq.Interval.InitializeAsync(stoppingToken),
                acq.DataTarget.InitializeAsync(stoppingToken)
            };

            //Add the list of additional inits
            initTasks.AddRange(acq.DataSources.Select(x => x.InitializeAsync(stoppingToken)));

            await Task.WhenAll(initTasks);

            //Wait for start trigger
            await acq.StartTrigger.WaitForTriggerAsync(stoppingToken);

            //Write config to Data Target
            await acq.DataTarget.WriteAcquisitionConfigAsync(acqConfig);

            //Acq started (or app cancelled)
            var endTrigger = acq.EndTrigger.WaitForTriggerAsync(stoppingToken);

            while (!endTrigger.IsCompleted & !stoppingToken.IsCancellationRequested)
            {
                //Wait for interval OR end trigger
                Task.WaitAny(acq.Interval.WaitForIntervalAsync(stoppingToken),
                             endTrigger);

                //If the end hasn't been triggered and we haven't been cancelled, get data
                if (!endTrigger.IsCompleted & !stoppingToken.IsCancellationRequested)
                {
                    //Get data from source(s)
                    var dataValues = await Task.WhenAll(acq.DataSources.Select(x => x.ReadDataAsync(stoppingToken)));

                    //Write data to target(s)
                    await acq.DataTarget.WriteDataAsync(dataValues, stoppingToken);

                }
                //Repeat @ Wait for interval OR end trigger

                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            //Not sure if needed - if cancellation we should wait for endtrigger to cancel
            await endTrigger;
        }
    }
}