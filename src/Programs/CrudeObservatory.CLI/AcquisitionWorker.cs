using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Abstractions.Models;
using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.Acquisition.Services;
using System.Threading.Channels;

namespace CrudeObservatory.CLI
{
    public class AcquisitionWorker : BackgroundService
    {
        private readonly ILogger<AcquisitionWorker> logger;
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly AcquisitionConfig acquisitionConfig;
        private readonly AcquisitionSetFactory acquisitionSetFactory;
        private readonly ChannelWriter<Measurement> channel;

        public AcquisitionWorker(
            ILogger<AcquisitionWorker> logger,
            IHostApplicationLifetime applicationLifetime,
            AcquisitionConfig acquisitionConfig,
            AcquisitionSetFactory acquisitionSetFactory,
            ChannelWriter<Measurement> channel
        )
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.applicationLifetime =
                applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            this.acquisitionConfig = acquisitionConfig ?? throw new ArgumentNullException(nameof(acquisitionConfig));
            this.acquisitionSetFactory =
                acquisitionSetFactory ?? throw new ArgumentNullException(nameof(acquisitionSetFactory));
            this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Build out classes - might be at the Program.cs DI level

            AcquisitionSet acq = acquisitionSetFactory.GetAcquisitionSet(acquisitionConfig);

            //Initiate connections? Could be DataSources (PLC) or DataTargets (DB) or Intervals(?)
            //Build a list of tasks to init so we can execute them in parallel
            var initTasks = new List<Task>()
            {
                acq.StartTrigger.InitializeAsync(stoppingToken),
                acq.EndTrigger.InitializeAsync(stoppingToken),
                acq.Interval.InitializeAsync(stoppingToken),
            };

            //Add the list of additional inits
            initTasks.AddRange(acq.DataSources.Select(x => x.InitializeAsync(stoppingToken)));
            initTasks.AddRange(acq.DataTargets.Select(x => x.InitializeAsync(stoppingToken)));

            await Task.WhenAll(initTasks);

            //Wait for start trigger
            await acq.StartTrigger.WaitForTriggerAsync(stoppingToken);

            //Write config to Data Targets
            await Task.WhenAll(
                acq.DataTargets.Select(x => x.WriteAcquisitionConfigAsync(acquisitionConfig, stoppingToken))
            );

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

                    var combinedDataValues = dataValues.SelectMany(x => x);

                    //Write data to channel
                    await channel.WriteAsync(
                        new Measurement
                        {
                            DataValues = combinedDataValues.ToList(),
                            IntervalOutput = intervalTask.Result
                        },
                        stoppingToken
                    );

                    //await Task.WhenAll(
                    //    acq.DataTargets.Select(
                    //        x => x.WriteDataAsync(intervalTask.Result, combinedDataValues, stoppingToken)
                    //    )
                    //);
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
