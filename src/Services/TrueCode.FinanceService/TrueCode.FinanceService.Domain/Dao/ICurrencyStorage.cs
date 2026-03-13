namespace TrueCode.FinanceService.Domain.Dao;

public interface ICurrencyStorage
{
    Task<List<string>> GetCurrenciesCodesAsync(CancellationToken cancellationToken = default);
}