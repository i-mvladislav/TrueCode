namespace TrueCode.FinanceService.Infrastructure.Configuration;

public sealed record JwtSettings
{
    public required string Key { get; init; }
    public required string Issuer { get; init; }
    public required string[] Audiences { get; init; }
}