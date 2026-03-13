using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueCode.FinanceService.Application.Currencies;
using TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesByUser;
using TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesCodes;

namespace TrueCode.FinanceService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddQueries(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<GetCurrenciesCodesQueryHandler>();
        services.AddScoped<GetCurrenciesByUserQueryHandler>();
        
        return services;
    }
}