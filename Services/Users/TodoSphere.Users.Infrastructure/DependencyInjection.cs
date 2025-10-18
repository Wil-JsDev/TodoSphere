using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TodoSphere.Users.Application.Interfaces.Repositories;
using TodoSphere.Users.Infrastructure.Context;
using TodoSphere.Users.Infrastructure.Repositories;

namespace TodoSphere.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region DBContext

        services.AddDbContext<TodoSphereUserContext>(option =>
        {
            var connectionString = configuration.GetConnectionString("ConnectionMongoDB");

            var client = new MongoClient(connectionString);

            option.UseMongoDB(client, "TodoSphereUsers");
        });

        #endregion

        #region Repositories

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRolesRepository, RolesRepository>();

        #endregion

        return services;
    }
}