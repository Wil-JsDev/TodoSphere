using Microsoft.Extensions.DependencyInjection;
using TodoSphere.Auth.Application.Interfaces.Services;
using TodoSphere.Auth.Application.Services;

namespace TodoSphere.Auth.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();
    }
}