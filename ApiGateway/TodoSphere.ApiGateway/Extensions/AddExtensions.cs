using Grafana.OpenTelemetry;
using OpenTelemetry.Logs;
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


    public static void AddOpenTelemetryTracing(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddOpenTelemetry()
            .WithTracing(configure =>
            {
                configure.UseGrafana()
                    .AddConsoleExporter();
            });
    }
    
    public static void AddOpenTelemetryLogging(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "ApiGateway";

        services.AddLogging(logging =>
        {
            logging.ClearProviders(); 
            logging.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(
                    OpenTelemetry.Resources.ResourceBuilder.CreateDefault()
                        .AddService(serviceName));

                options.UseGrafana(); 
                options.AddConsoleExporter(); 
                options.AddOtlpExporter(opt => { opt.Endpoint = new Uri("http://loki:3100"); });
            });
        });
    }
}