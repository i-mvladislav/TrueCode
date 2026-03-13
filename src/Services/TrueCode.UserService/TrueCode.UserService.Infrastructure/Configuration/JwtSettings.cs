namespace TrueCode.UserService.Infrastructure.Configuration;

public sealed record JwtSettings
{
    public required string Key { get; init; }
    public required string Issuer { get; init; }
    public required string[] Audiences { get; init; }
}