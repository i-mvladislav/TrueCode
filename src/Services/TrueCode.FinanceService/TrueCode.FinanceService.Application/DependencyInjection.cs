using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueCode.Core.Queries;
using TrueCode.FinanceService.Application.Currencies.Models;
using TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesByUser;
using TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesCodes;

namespace TrueCode.FinanceService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddQueries(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<BaseQueryHandler<GetCurrenciesCodesQuery, List<string>>, GetCurrenciesCodesQueryHandler>();
        services.AddScoped<BaseQueryHandler<GetCurrenciesByUserQuery, List<Currency>>, GetCurrenciesByUserQueryHandler>();
        
        return services;
    }
}