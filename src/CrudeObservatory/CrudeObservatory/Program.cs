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
        //TODO: Should come from command line
        var jsonConfig = File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "AcqConfig.json"));

        services.AddSingleton<ParseAcquisitionConfig>();

        //We do this b/c in future we may want multiplem workers running different acq configs
        services.AddHostedService<Worker>(x => 
            new Worker(x.GetRequiredService<ILogger<Worker>>(), 
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
