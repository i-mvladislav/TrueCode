using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Yarp.ReverseProxy.Swagger;
using Yarp.ReverseProxy.Swagger.Extensions;

namespace TrueCode.ApiGateway;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();

        var yarpProxySection = configuration.GetSection("YarpProxy");
        services.AddReverseProxy()
            .LoadFromConfig(yarpProxySection)
            .AddSwagger(yarpProxySection);

        services.AddRateLimiter(options =>
        {
            options.AddPolicy("PerClientPolicy", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromSeconds(3)
                    }
                ));

            options.OnRejected = (context, token) =>
            {
                var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger?.LogWarning("Лимит запросов превышен для IP: {IP} | {Path}",
                    ip,
                    context.HttpContext.Request.Path);
                return ValueTask.CompletedTask;
            };
        });

        return services;
    }
    
    public static WebApplication UseApiServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var config = app.Services.GetRequiredService<IOptionsMonitor<ReverseProxyDocumentFilterConfig>>().CurrentValue;
                options.ConfigureSwaggerEndpoints(config);
            });
        }
        
        app.UseRateLimiter();
        app.MapReverseProxy()
            .RequireRateLimiting("PerClientPolicy");

        return app;
    }
}