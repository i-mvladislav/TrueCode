namespace TrueCode.UserService.Application.Currencies.Commands.RemoveFavoriteCurrency;

public sealed record RemoveFavoriteCurrencyCommand
{
    public required string Name { get; init; }
}