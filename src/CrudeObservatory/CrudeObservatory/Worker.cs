using CrudeObservatory.Acquisition.Models;
using Microsoft.Extensions.Options;

namespace CrudeObservatory
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly AcquisitionConfig acquisitionConfig;

        public Worker(ILogger<Worker> logger, IHostApplicationLifetime applicationLifetime, AcquisitionConfig acquisitionConfig)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            this.acquisitionConfig = acquisitionConfig ?? throw new ArgumentNullException(nameof(acquisitionConfig));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Build out classes - might be at the Program.cs DI level

            AcquisitionSet acq = ManualAcqSet.GetAcquisition(acquisitionConfig);

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
            await acq.DataTarget.WriteAcquisitionConfigAsync(acquisitionConfig, stoppingToken);

            //Acq started (or app cancelled)
            var endTrigger = acq.EndTrigger.WaitForTriggerAsync(stoppingToken);

            while (!endTrigger.IsCompleted & !stoppingToken.IsCancellationRequested)
            {
                //Wait for interval OR end trigger
                var intervalTask = acq.Interval.WaitForIntervalAsync(stoppingToken);
                Task.WaitAny(intervalTask, endTrigger);

                logger.LogInformation("Interval trigger [{@interval}] fired", acquisitionConfig.Interval);

                //If the end hasn't been triggered and we haven't been cancelled, get data
                if (!endTrigger.IsCompleted & !stoppingToken.IsCancellationRequested)
                {
                    //Get data from source(s)
                    var dataValues = await Task.WhenAll(acq.DataSources.Select(x => x.ReadDataAsync(stoppingToken)));

                    var combinedDataValues = intervalTask.Result.ToList();

                    foreach (var item in dataValues)
                    {
                        combinedDataValues.AddRange(item);
                    }

                    //Write data to target(s)
                    await acq.DataTarget.WriteDataAsync(combinedDataValues, stoppingToken);

                }
                //Repeat @ Wait for interval OR end trigger

            }

            //Not sure if needed - if cancellation we should wait for endtrigger to cancel
            await endTrigger;
            logger.LogInformation("End trigger [{@EndTrigger}] fired", acquisitionConfig.EndTrigger);
            applicationLifetime.StopApplication();

        }
    }
}