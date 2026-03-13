namespace TrueCode.FinanceService.Infrastructure.Configuration;

public sealed record UrlsSettings
{
    public required string Cbr { get; init; }
    public required string UserService { get; init; }
}