using Grafana.OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace TodoSphere.ApiGateway.Extensions;

public static class ServiceExtension
{
    public static void AddLoggingOpenTelemetry(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "ApiGateway";

        builder.Logging.ClearProviders();

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName));

            options.UseGrafana(); // Exporta a Grafana Loki
            options.AddConsoleExporter(); // Logs locales opcionales

            options.AddOtlpExporter(opt => { opt.Endpoint = new Uri("http://loki:3100"); });
        });
    }

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