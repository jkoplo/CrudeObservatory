using CrudeObservatory;
using CrudeObservatory.Acquisition.Models;
using CrudeObservatory.Acquisition.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");
Log.Information("AppContext.BaseDirectory: {path}", AppContext.BaseDirectory);

try
{
    IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration))
    .ConfigureServices((hostContext, services) =>
    {
        //Determine what json we want for config
        var acqConfigPath = Path.GetFullPath(hostContext.Configuration["AcqConfigPath"]);
        Log.Information("Loading Acquisition Config from: {path}", acqConfigPath);
        var jsonConfig = File.ReadAllText(acqConfigPath);

        services.AddSingleton<ParseAcquisitionConfig>();

        //We do this b/c in future we may want multiplem workers running different acq configs
        services.AddHostedService<AcquisitionWorker>(x =>
            new AcquisitionWorker(x.GetRequiredService<ILogger<AcquisitionWorker>>(),
                                  x.GetRequiredService<IHostApplicationLifetime>(),
                                  x.GetRequiredService<ParseAcquisitionConfig>().DeserializeFromJson(jsonConfig)));
    })
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
