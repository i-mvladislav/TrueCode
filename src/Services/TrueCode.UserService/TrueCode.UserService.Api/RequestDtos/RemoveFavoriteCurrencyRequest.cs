namespace TrueCode.UserService.Api.RequestDtos;

public sealed record RemoveFavoriteCurrencyRequest
{
    public required string Name { get; init; }
}