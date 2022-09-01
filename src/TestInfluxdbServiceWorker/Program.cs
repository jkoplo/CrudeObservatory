using InfluxDbManager;
using TestInfluxdbServiceWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<InfluxdbWorker>();
        services.AddTransient<InfluxdbClient>();
        services.AddHostedService<SingleInfluxdbAcqWorker>();
    })
    .Build();

await host.RunAsync();
