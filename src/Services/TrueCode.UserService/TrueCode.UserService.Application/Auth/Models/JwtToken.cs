namespace TrueCode.UserService.Application.Auth.Models;

public sealed record JwtToken
{
    public required string AccessToken { get; init; }
    public required DateTimeOffset Expires { get; init; }
}