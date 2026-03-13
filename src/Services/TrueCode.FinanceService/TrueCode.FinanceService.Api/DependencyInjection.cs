using System.Text;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TrueCode.FinanceService.Application;
using TrueCode.FinanceService.Infrastructure;
using TrueCode.FinanceService.Infrastructure.Configuration;
using TrueCode.UserService.HttpClients;

namespace TrueCode.FinanceService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();
        services.AddJobServices(configuration);
        services.AddHttpClients(configuration);
        services.AddAuth(configuration);
        services.AddDatabase(configuration);
        services.AddStorages(configuration);
        services.AddQueries(configuration);

        return services;
    }
    
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Jwt").Get<JwtSettings>()!;

        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.Issuer,
                    ValidAudiences = settings.Audiences,
                    IssuerSigningKey = key,
                };
            });
        
        return services;
    }
    
    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapCarter();
        
        return app;
    }
}