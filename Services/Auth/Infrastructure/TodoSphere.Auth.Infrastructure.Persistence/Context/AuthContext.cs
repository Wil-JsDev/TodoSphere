using Microsoft.EntityFrameworkCore;
using TodoSphere.Auth.Domain.Models;

namespace TodoSphere.Auth.Persistence.Context;

public class AuthContext(DbContextOptions<AuthContext> options) : DbContext(options)
{
    #region Models

    public DbSet<User> Users { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Tables

        modelBuilder.Entity<User>()
            .ToTable("Users");

        modelBuilder.Entity<RefreshToken>()
            .ToTable("RefreshTokens");

        #endregion

        #region PKs

        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId)
            .HasName("PK_Users");

        modelBuilder.Entity<RefreshToken>()
            .HasKey(rt => rt.RefreshTokenId)
            .HasName("PK_RefreshTokens");

        #endregion

        #region Relationships

        modelBuilder.Entity<User>()
            .HasMany(us => us.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .IsRequired()
            .HasConstraintName("FK_RefreshTokens_Users")
            .OnDelete(DeleteBehavior.Cascade);

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

            entity.Property(u => u.Roles)
                .IsRequired()
                .HasMaxLength(50);

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
    }
}