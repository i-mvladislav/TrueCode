using TrueCode.FinanceService.Domain.Entities;

namespace TrueCode.FinanceService.Domain.Dao;

public interface ICurrencyStorage
{
    Task<List<string>> GetCurrenciesCodesAsync(CancellationToken cancellationToken = default);
    Task<List<CurrencyEntity>> GetCurrenciesByCodesAsync(IEnumerable<string> currenciesCodes, CancellationToken cancellationToken = default);
}