namespace TrueCode.UserService.Api.RequestDtos;

public sealed record AddFavoriteCurrencyRequest
{
    public required string Name { get; init; }
}