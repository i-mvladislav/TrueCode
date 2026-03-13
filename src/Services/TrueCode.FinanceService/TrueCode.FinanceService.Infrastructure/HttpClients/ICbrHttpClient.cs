using Refit;
using TrueCode.FinanceService.Infrastructure.Dtos;

namespace TrueCode.FinanceService.Infrastructure.HttpClients;

public interface ICbrHttpClient
{
    [Get("/scripts/XML_daily.asp")]
    public Task<CbrResponse> GetCurrenciesAsync(CancellationToken cancellationToken = default);
}