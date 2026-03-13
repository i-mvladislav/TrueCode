using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueCode.UserService.Application.Auth.Commands.SignIn;
using TrueCode.UserService.Application.Auth.Commands.SignUp;
using TrueCode.UserService.Application.Currencies.Commands.AddFavoriteCurrency;
using TrueCode.UserService.Application.Currencies.Commands.RemoveFavoriteCurrency;
using TrueCode.UserService.Application.Currencies.Queries;

namespace TrueCode.UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCommands(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<SignInCommandHandler>();
        services.AddScoped<SignUpCommandHandler>();

        services.AddScoped<AddFavoriteCurrencyCommandHandler>();
        services.AddScoped<RemoveFavoriteCurrencyCommandHandler>();

        return services;
    }

    public static IServiceCollection AddQueries(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<GetFavoriteCurrenciesQueryHandler>();
        
        return services;
    }
}