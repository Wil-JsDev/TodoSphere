using Grafana.OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TodoSphere.ApiGateway.Extensions;

public static class AddExtensions
{
    public static void AddReverseProxyConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));
    }

    public static void AddOpenTelemetryMetrics(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "ApiGateway";

        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName));

                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();
                metrics.AddRuntimeInstrumentation();

                // Exportador Prometheus (para que Prometheus lo pueda scrapear)
                metrics.AddPrometheusExporter();
            });
    }

    // SE QUEDA (Configuración de Tracing para Grafana)
    public static void AddOpenTelemetryTracing(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .WithTracing(configure =>
            {
                configure.UseGrafana()
                    .AddConsoleExporter();
            });
    }
}