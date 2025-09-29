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

    // Open Telemetry Metrics + Traces
    public static void AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "ApiGateway";
        var otlpEndpoint = configuration["OpenTelemetry:OtlExporter:Endpoint"] ?? "http://grafana-agent:4317";

        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName));
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddHttpClientInstrumentation();
                metrics.AddOtlpExporter(opt => { opt.Endpoint = new Uri(otlpEndpoint); });
            })
            .WithTracing(tracing =>
            {
                tracing.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName));
                tracing.AddAspNetCoreInstrumentation();
                tracing.AddHttpClientInstrumentation();
                tracing.AddOtlpExporter(opt => { opt.Endpoint = new Uri(otlpEndpoint); });
            });
    }
}