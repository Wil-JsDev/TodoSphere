using Microsoft.OpenApi.Models;

namespace TodoSphere.Auth.API.Extensions;

public static class ServiceExtension
{
    public static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "TodoSphere Auth service Api",
                Description = "Service Api for Auth",
                Contact = new OpenApiContact
                {
                    Name = "Wilmer De La Cruz"
                }
            });
        });
    }
}