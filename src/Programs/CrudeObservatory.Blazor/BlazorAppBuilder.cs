
using CrudeObservatory.Blazor.Data;

namespace ChromelyBlazor.ServerApp;

public sealed class BlazorAppBuilder
{
    private readonly WebApplicationBuilder _hostBuilder;
    private WebApplication? _host;

    private BlazorAppBuilder(WebApplicationBuilder hostBuilder)
    {
        _hostBuilder = hostBuilder;
    }

    public static BlazorAppBuilder Create(string[] args, int port)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<WeatherForecastService>();

        //var hostBuilder = Host.CreateDefaultBuilder(args)
        //    .ConfigureWebHostDefaults(webBuilder =>
        //    {
        //        webBuilder
        //        .UseStartup<Startup>()
        //        .UseUrls(new[] { $"https://127.0.0.1:{port}" });
        //    });

        var appBuilder = new BlazorAppBuilder(builder);

        return appBuilder;
    }

    public BlazorAppBuilder Build()
    {

        _host = _hostBuilder.Build();
        return this;
    }


    public void Run(int port)
    {
        // Configure the HTTP request pipeline.
        if (!_host.Environment.IsDevelopment())
        {
            _host.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _host.UseHsts();
        }

        _host.UseHttpsRedirection();

        _host.UseStaticFiles();
        _host.UseRouting();

        _host.MapBlazorHub();
        _host.MapFallbackToPage("/_Host");

        _host?.Run($"https://127.0.0.1:{port}");
    }
}