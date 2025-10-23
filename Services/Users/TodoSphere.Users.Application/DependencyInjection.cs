using Microsoft.Extensions.DependencyInjection;
using TodoSphere.Users.Application.Interfaces.Services;
using TodoSphere.Users.Application.Services;

namespace TodoSphere.Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRolesService, RolesService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}