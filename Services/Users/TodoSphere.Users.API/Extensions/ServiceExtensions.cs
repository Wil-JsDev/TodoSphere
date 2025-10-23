using Microsoft.OpenApi.Models;

namespace TodoSphere.Users.API.Extensions;

public static class ServiceExtensions
{
    public static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "TodoSphere User service Api",
                Description = "",
                Contact = new OpenApiContact
                {
                    Name = "Wilmer De La Cruz"
                }
            });
            option.EnableAnnotations();
        });
    }
}