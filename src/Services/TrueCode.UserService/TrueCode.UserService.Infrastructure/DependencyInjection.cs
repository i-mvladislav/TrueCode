using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Infrastructure.Dao;
using TrueCode.UserService.Infrastructure.Data;

namespace TrueCode.UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PgConnection")!;

        services.AddDbContext<ApplicationContext>(opts =>
            opts.UseNpgsql(connectionString, builder => builder.MigrationsAssembly("TrueCode.UserService.Infrastructure")));
        
        return services;
    }

    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        return services;
    }

    public static IServiceCollection AddStorages(this IServiceCollection services)
    {
        services.AddScoped<IUserStorage, UserStorage>();
        services.AddScoped<ICurrencyStorage, CurrencyStorage>();

        return services;
    }
}