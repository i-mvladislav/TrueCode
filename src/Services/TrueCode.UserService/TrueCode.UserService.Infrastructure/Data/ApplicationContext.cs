using Microsoft.EntityFrameworkCore;
using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Infrastructure.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<FavoriteCurrencyEntity> FavouriteCurrencies => Set<FavoriteCurrencyEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasKey(u => u.Id);
        
        modelBuilder.Entity<UserEntity>()
            .HasMany(u => u.FavouriteCurrencies)
            .WithOne(f => f.User)
            .HasForeignKey(f => f.UserId);
        
        modelBuilder.Entity<FavoriteCurrencyEntity>()
            .HasKey(f => new { f.UserId, CurrencyId = f.Code });

        modelBuilder.Entity<UserEntity>()
            .ToTable("user");
        
        base.OnModelCreating(modelBuilder);
    }
}