using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoSphere.Auth.Application.Interfaces.Repositories;
using TodoSphere.Auth.Persistence.Context;
using TodoSphere.Auth.Persistence.Repository;

namespace TodoSphere.Auth.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        #region DbContext

        services.AddDbContext<AuthContext>(postgres =>
        {
            postgres.UseNpgsql(configuration.GetConnectionString("AuthConnection"),
                option => { option.MigrationsAssembly("TodoSphere.Auth.Infrastructure.Persistence"); });
        });

        #endregion

        services.AddRepositories();

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRepository<>), typeof(Repository.Repository<>));
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddTransient<IRolRepository, RolRepository>();
    }
}