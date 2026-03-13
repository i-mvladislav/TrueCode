using System.Text;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TrueCode.UserService.Api.Middlewares;
using TrueCode.UserService.Api.Services;
using TrueCode.UserService.Application;
using TrueCode.UserService.Application.Contracts;
using TrueCode.UserService.Infrastructure;
using TrueCode.UserService.Infrastructure.Auth;
using TrueCode.UserService.Infrastructure.Configuration;

namespace TrueCode.UserService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);
        services.AddCarter();
        services.AddSingleton<JwtBlacklistService>();

        services.AddScoped<IJwtService, JwtService>();
        
        services.AddCommands(configuration);
        services.AddDatabase(configuration);
        services.AddCaching();
        services.AddQueries(configuration);
        services.AddStorages();
        
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
        
        app.UseMiddleware<JwtBlacklistMiddleware>();
        
        app.MapCarter();
        
        return app;
    }
}