namespace TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesByUser;

public sealed record GetCurrenciesByUserQuery
{
    public required Guid UserId { get; init; }
    public required string JwtToken { get; init; }
}