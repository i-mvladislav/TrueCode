using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueCode.Core.Commands;
using TrueCode.Core.Queries;
using TrueCode.UserService.Application.Auth.Commands.SignIn;
using TrueCode.UserService.Application.Auth.Commands.SignUp;
using TrueCode.UserService.Application.Auth.Models;
using TrueCode.UserService.Application.Currencies.Commands.AddFavoriteCurrency;
using TrueCode.UserService.Application.Currencies.Commands.RemoveFavoriteCurrency;
using TrueCode.UserService.Application.Currencies.Queries;

namespace TrueCode.UserService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCommands(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<BaseCommandHandler<SignInCommand, JwtToken>, SignInCommandHandler>();
        services.AddScoped<BaseCommandHandler<SignUpCommand>, SignUpCommandHandler>();

        services.AddScoped<BaseCommandHandler<AddFavoriteCurrencyCommand>, AddFavoriteCurrencyCommandHandler>();
        services.AddScoped<BaseCommandHandler<RemoveFavoriteCurrencyCommand>, RemoveFavoriteCurrencyCommandHandler>();

        return services;
    }

    public static IServiceCollection AddQueries(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<BaseQueryHandler<GetFavoriteCurrenciesQuery, List<string>>, GetFavoriteCurrenciesQueryHandler>();
        
        return services;
    }
}