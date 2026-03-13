using Microsoft.EntityFrameworkCore;
using TrueCode.FinanceService.Domain.Dao;
using TrueCode.FinanceService.Domain.Entities;
using TrueCode.FinanceService.Infrastructure.Data;

namespace TrueCode.FinanceService.Infrastructure.Dao;

public sealed class CurrencyStorage(ApplicationContext db) : ICurrencyStorage
{
    public Task<List<string>> GetCurrenciesCodesAsync(CancellationToken cancellationToken = default)
    {
        return db.Currencies.Select(c => c.Id).ToListAsync(cancellationToken);
    }

    public Task<List<CurrencyEntity>> GetCurrenciesByCodesAsync(IEnumerable<string> currenciesCodes, CancellationToken cancellationToken = default)
    {
        return db.Currencies.Where(c => currenciesCodes.Contains(c.Name)).ToListAsync(cancellationToken);
    }
}