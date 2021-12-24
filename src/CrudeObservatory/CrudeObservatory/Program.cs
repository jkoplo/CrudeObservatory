using CrudeObservatory;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog((ctx, lc) => lc
        //.WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration))
    .ConfigureServices(services =>
    {
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