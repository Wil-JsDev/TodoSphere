using Serilog;
using TodoSphere.ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Reverse Proxy
builder.Services.AddReverseProxyConfiguration(builder.Configuration);

// Open Telemetry Metrics
builder.Services.AddOpenTelemetryMetrics(builder.Configuration);

// Open Telemetry Tracing
builder.Services.AddOpenTelemetryTracing(builder.Configuration);


// Loki + Serilog
builder.AddSerilogWithLoki();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// Exponer el endpoint para Prometheus
app.MapPrometheusScrapingEndpoint("/metrics");

app.MapReverseProxy();

app.Run();