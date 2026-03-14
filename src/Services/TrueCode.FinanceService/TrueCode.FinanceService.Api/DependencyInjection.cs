using System.Text;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TrueCode.Core.Users;
using TrueCode.FinanceService.Api.Security;
using TrueCode.FinanceService.Application;
using TrueCode.FinanceService.Infrastructure;
using TrueCode.FinanceService.Infrastructure.Configuration;

namespace TrueCode.FinanceService.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opts =>
        {
            opts.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme.",
            });
            
            opts.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });
        });
        
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();
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
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapCarter();
        
        return app;
    }
}