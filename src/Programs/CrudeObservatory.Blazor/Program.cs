#pragma warning disable CS8622 

using Chromely;
using Chromely.Core;
using Chromely.Core.Configuration;
using ChromelyBlazor.ServerApp;
using ChromelyBlazor.Shared;
using ChromelyBlazor.Shared.ChromelyControllers;
using System.Reflection;

var appName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Chromely Blazor Server";
var firstProcess = ServerAppUtil.IsMainProcess(args);
var port = ServerAppUtil.AvailablePort;

if (firstProcess)
{
    if (port != -1)
    {
        // start the kestrel server in a background thread
        AppDomain.CurrentDomain.ProcessExit += ServerAppUtil.ProcessExit;
        ServerAppUtil.BlazorTaskTokenSource = new CancellationTokenSource();
        ServerAppUtil.BlazorTask = new Task(() =>
        {
            BlazorAppBuilder.Create(args, port)
                .Build()
                .Run(port);

        }, ServerAppUtil.BlazorTaskTokenSource.Token, TaskCreationOptions.LongRunning);
        ServerAppUtil.BlazorTask.Start();

        // wait till its up
        while (ServerAppUtil.IsPortAvailable(port))
        {
            Thread.Sleep(1);
        }
    }

    // Save port for later use by chromely processes
    ServerAppUtil.SavePort(appName, port);
}
else
{
    // fetch port number
    port = ServerAppUtil.GetSavedPort(appName);
}

if (port != -1)
{
    // start up chromely
    var core = typeof(IChromelyConfiguration).Assembly;
    var config = DefaultConfiguration.CreateForRuntimePlatform();
    config.WindowOptions.Title = "blazor server app demo";
    config.StartUrl = $"https://127.0.0.1:{port}";
    config.DebuggingMode = true;
    config.WindowOptions.RelativePathToIconFile = "chromely.ico";

    try
    {
        var builderChromely = AppBuilder.Create(args);
        builderChromely = builderChromely.UseConfig<DefaultConfiguration>(config);
        builderChromely = builderChromely.UseApp<DemoChromelyApp>();
        builderChromely = builderChromely.Build();
        builderChromely.Run();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        throw;
    }
}


public class DemoChromelyApp : ChromelyBasicApp
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        RegisterChromelyControllerAssembly(services, typeof(DemoController).Assembly);
    }
}
