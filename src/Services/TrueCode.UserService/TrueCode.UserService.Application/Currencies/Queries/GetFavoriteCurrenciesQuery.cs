namespace TrueCode.UserService.Application.Currencies.Queries;

public sealed record GetFavoriteCurrenciesQuery
{
    public required Guid UserId { get; set; }
}