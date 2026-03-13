using Microsoft.EntityFrameworkCore;
using TrueCode.FinanceService.Domain.Entities;

namespace TrueCode.FinanceService.Infrastructure.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<CurrencyEntity> Currencies => Set<CurrencyEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyEntity>().HasKey(c => c.Id);

        modelBuilder.Entity<CurrencyEntity>()
            .Property(c => c.Name)
            .IsRequired();

        modelBuilder.Entity<CurrencyEntity>()
            .Property(c => c.Rate)
            .HasColumnType("decimal(18,6)");

        modelBuilder.Entity<CurrencyEntity>()
            .ToTable("currency");
        
        base.OnModelCreating(modelBuilder);
    }
}