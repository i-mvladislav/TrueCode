using System.Text;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Refit;
using TrueCode.FinanceService.Domain.Dao;
using TrueCode.FinanceService.Infrastructure.Configuration;
using TrueCode.FinanceService.Infrastructure.Dao;
using TrueCode.FinanceService.Infrastructure.Data;
using TrueCode.FinanceService.Infrastructure.HttpClients;
using TrueCode.FinanceService.Infrastructure.Jobs;
using TrueCode.UserService.HttpClients;

namespace TrueCode.FinanceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddJobServices(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Job").Get<JobSettings>()!;
        
        services.AddQuartz(q =>
        {
            q.AddJob<CloneValutesJob>(opts => opts.WithIdentity(CloneValutesJob.Key));

            q.AddTrigger(opts => opts
                .ForJob(CloneValutesJob.Key)
                .WithIdentity("startup-trigger")
                .StartNow()
            );
            
            q.AddTrigger(opts => opts
                .ForJob(CloneValutesJob.Key)
                .WithIdentity("cron-trigger")
                .WithCronSchedule(settings.Cron)
            );
        });
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        
        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Urls").Get<UrlsSettings>()!;

        var cbrSettings = new RefitSettings
        {
            ContentSerializer = new XmlContentSerializer(
                new XmlContentSerializerSettings
                {
                    XmlReaderWriterSettings = new XmlReaderWriterSettings
                    {
                        WriterSettings = new XmlWriterSettings
                        {
                            Encoding = Encoding.GetEncoding("windows-1251"),
                        },
                    },
                }
            )
        };
        services
            .AddRefitClient<ICbrHttpClient>(cbrSettings)
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.Cbr));
        
        services.AddUserServiceHttpClients(settings.UserService);

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PgConnection")!;

        services.AddDbContext<ApplicationContext>(opts =>
            opts.UseNpgsql(connectionString, builder => builder.MigrationsAssembly("TrueCode.FinanceService.Infrastructure")));
        
        return services;
    }

    public static IServiceCollection AddStorages(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICurrencyStorage, CurrencyStorage>();
        return services;
    }
}