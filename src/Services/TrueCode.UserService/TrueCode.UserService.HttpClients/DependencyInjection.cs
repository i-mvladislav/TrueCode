using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace TrueCode.UserService.HttpClients;

public static class DependencyInjection
{
    public static IServiceCollection AddUserServiceHttpClients(this IServiceCollection services, string baseAddress)
    {
        services.AddRefitClient<ICurrenciesHttpClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseAddress));
        
        return services;
    }
}