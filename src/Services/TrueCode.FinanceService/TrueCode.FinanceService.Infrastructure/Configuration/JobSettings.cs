namespace TrueCode.FinanceService.Infrastructure.Configuration;

public sealed record JobSettings
{
    public required string Cron { get; init; }
}