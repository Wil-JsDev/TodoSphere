using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace TodoSphere.ApiGateway.Extensions;

public static class ServiceExtension
{
    public static void AddSerilogWithLoki(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("service", "ApiGateway") // name service
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.GrafanaLoki("http://loki:3100"); // hostname intern in docker
        });
    }
}