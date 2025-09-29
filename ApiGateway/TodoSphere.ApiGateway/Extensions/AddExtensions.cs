namespace TodoSphere.ApiGateway.Extensions;

public static class AddExtensions
{
    public static void AddReverseProxyConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));
    }
}