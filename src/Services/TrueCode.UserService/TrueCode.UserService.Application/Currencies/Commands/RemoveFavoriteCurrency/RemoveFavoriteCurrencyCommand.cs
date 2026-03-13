namespace TrueCode.UserService.Application.Currencies.Commands.RemoveFavoriteCurrency;

public sealed record RemoveFavoriteCurrencyCommand
{
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
}