using Microsoft.EntityFrameworkCore;
using TodoSphere.Auth.Domain.Models;

namespace TodoSphere.Auth.Persistence.Context;

public class AuthContext(DbContextOptions<AuthContext> options) : DbContext(options)
{
    #region Models

    public DbSet<User> Users { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public DbSet<Roles> Roles { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Tables

        modelBuilder.Entity<User>()
            .ToTable("Users");

        modelBuilder.Entity<RefreshToken>()
            .ToTable("RefreshTokens");

        modelBuilder.Entity<Roles>()
            .ToTable("Roles");

        #endregion

        #region PKs

        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId)
            .HasName("PK_Users");

        modelBuilder.Entity<RefreshToken>()
            .HasKey(rt => rt.RefreshTokenId)
            .HasName("PK_RefreshTokens");

        modelBuilder.Entity<Roles>()
            .HasKey(r => r.RoleId)
            .HasName("PK_Roles");

        #endregion

        #region Relationships

        modelBuilder.Entity<User>()
            .HasMany(us => us.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .IsRequired()
            .HasConstraintName("FK_RefreshTokens_Users")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(us => us.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(us => us.RoleId)
            .IsRequired()
            .HasConstraintName("FK_Users_Roles")
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region Users

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(u => u.Email)
                .IsUnique()
                .HasName("IX_Users_Email");

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(us => us.IsEmailVerified)
                .IsRequired()
                .HasDefaultValue(false);
        });

        #endregion

        #region RefreshTokens

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(300);

            entity.HasIndex(rt => rt.Token)
                .IsUnique()
                .HasName("IX_RefreshTokens_Token");

            entity.Property(rt => rt.ExpiresAt)
                .IsRequired();

            entity.Property(rt => rt.RevokedAt)
                .IsRequired(false);
        });

        #endregion

        #region Roles

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        #endregion
    }
}