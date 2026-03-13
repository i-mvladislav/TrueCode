using TrueCode.Core.Queries;
using TrueCode.FinanceService.Domain.Dao;

namespace TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesCodes;

public class GetCurrenciesCodesQueryHandler(ICurrencyStorage currencyStorage) : BaseQueryHandler<GetCurrenciesCodesQuery, List<string>>
{
    protected override async Task<QueryResult<List<string>>> ExecuteCoreAsync(GetCurrenciesCodesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await currencyStorage.GetCurrenciesCodesAsync(cancellationToken);
        return QueryResult<List<string>>.Success(result);
    }
}