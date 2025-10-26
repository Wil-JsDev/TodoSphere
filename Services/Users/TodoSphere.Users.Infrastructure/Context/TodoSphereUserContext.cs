using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using TodoSphere.Users.Domain.Models;

namespace TodoSphere.Users.Infrastructure.Context;

public sealed class TodoSphereUserContext(DbContextOptions<TodoSphereUserContext> options) : DbContext(options)
{
    public static TodoSphereUserContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<TodoSphereUserContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName).Options);

    #region Models

    public DbSet<User> Users { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Collections

        modelBuilder.Entity<User>().ToCollection("Users");

        #endregion

        #region PKs

        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<User>()
            .Property(u => u.UserId)
            .HasElementName("_id");

        #endregion

        #region User

        modelBuilder.Entity<User>(property =>
        {
            property.Property(user => user.CreatedAt)
                .IsRequired();

            property.Property(user => user.UpdatedAt)
                .IsRequired(false);

            // property.OwnsMany(u => u.Roles, builder =>
            // {
            //     builder.Property(roleInfo => roleInfo.RoleId).HasElementName("roleId");
            //     builder.Property(roleInfo => roleInfo.RoleName).HasElementName("name");
            //     builder.Property(roleInfo => roleInfo.AssignedAt).HasElementName("assignedAt");
            // });
        });

        #endregion
    }
}