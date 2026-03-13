namespace TrueCode.UserService.Application.Currencies.Commands.AddFavoriteCurrency;

public sealed record AddFavoriteCurrencyCommand
{
    public required Guid UserId { get; init; }
    public required string CurrencyCode { get; init; }
}