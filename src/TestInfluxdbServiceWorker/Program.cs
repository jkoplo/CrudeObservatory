using InfluxDbManager;
using TestInfluxdbServiceWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<InfluxdbWorker>();
    })
    .Build();

await host.RunAsync();
