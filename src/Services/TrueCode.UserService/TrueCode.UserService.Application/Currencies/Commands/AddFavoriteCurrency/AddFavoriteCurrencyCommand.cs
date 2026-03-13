namespace TrueCode.UserService.Application.Currencies.Commands.AddFavoriteCurrency;

public sealed record AddFavoriteCurrencyCommand
{
    public required string Name { get; init; }
}