using CrudeObservatory;
using CrudeObservatory.Acquisition.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration))
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<AcquisitionConfig>(ManualAcqSet.GetAcquisitionConfig());
        //services.Configure<AcquisitionConfig>(hostContext.Configuration.GetSection("Acquisition"));
        services.AddHostedService<Worker>();
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
