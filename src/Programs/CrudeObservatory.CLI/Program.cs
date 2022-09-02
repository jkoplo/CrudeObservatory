using CrudeObservatory.Abstractions.Interfaces;
using CrudeObservatory.Acquisition.Services;
using CrudeObservatory.CLI;
using InfluxDbManager;
using Serilog;
using System.Threading.Channels;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

Log.Information("Starting up");
Log.Information("AppContext.BaseDirectory: {path}", AppContext.BaseDirectory);

try
{
    IHost host = Host.CreateDefaultBuilder(args)
        .UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration))
        .ConfigureServices(
            (hostContext, services) =>
            {
                //Determine what json we want for config
                var acqConfigPath = Path.GetFullPath(hostContext.Configuration["AcqConfigPath"]);
                Log.Information("Loading Acquisition Config from: {path}", acqConfigPath);
                var jsonConfig = File.ReadAllText(acqConfigPath);

                services.AddSingleton<ParseAcquisitionConfig>();
                services.AddSingleton<AcquisitionSetFactory>();

                //Add the CLI client and the Daemon for Influx
                services.AddTransient<InfluxdbDaemon>();
                services.AddTransient<InfluxdbCliClient>();
                services.AddHostedService<InfluxdbWorker>();
                services.AddHostedService<DataTargetWorker>();

                var channel = Channel.CreateBounded<Measurement>(100);
                services.AddSingleton<ChannelWriter<Measurement>>(channel);
                services.AddSingleton<ChannelReader<Measurement>>(channel);

                //We do this b/c in future we may want multiple workers running different acq configs
                services.AddHostedService<AcquisitionWorker>(
                    x =>
                        new AcquisitionWorker(
                            x.GetRequiredService<ILogger<AcquisitionWorker>>(),
                            x.GetRequiredService<IHostApplicationLifetime>(),
                            x.GetRequiredService<ParseAcquisitionConfig>().DeserializeFromJson(jsonConfig),
                            x.GetRequiredService<AcquisitionSetFactory>(),
                            x.GetRequiredService<ChannelWriter<Measurement>>()
                        )
                );
            }
        )
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
