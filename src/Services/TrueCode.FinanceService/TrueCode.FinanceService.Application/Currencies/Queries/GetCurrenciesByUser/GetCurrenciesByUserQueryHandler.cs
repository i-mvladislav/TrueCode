using System.Net;
using TrueCode.Core.Models;
using TrueCode.Core.Queries;
using TrueCode.FinanceService.Domain.Dao;
using TrueCode.FinanceService.Domain.Entities;
using TrueCode.UserService.HttpClients;

namespace TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesByUser;

public class GetCurrenciesByUserQueryHandler(ICurrenciesHttpClient client, ICurrencyStorage currencyStorage) : BaseQueryHandler<GetCurrenciesByUserQuery, List<CurrencyEntity>>
{
    protected override async Task<QueryResult<List<CurrencyEntity>>> ExecuteCoreAsync(GetCurrenciesByUserQuery query, CancellationToken cancellationToken = default)
    {
        List<Error> errors = [];
        
        if (query.UserId == Guid.Empty)
            errors.Add(new Error("Некорректный пользователь."));
        
        if (string.IsNullOrWhiteSpace(query.JwtToken))
            errors.Add(new Error("Некорректный токен."));
        
        if (errors.Count > 0)
            return QueryResult<List<CurrencyEntity>>.Failure(errors);
        
        var response = await client.GetFavoriteCurrenciesAsync(query.UserId, query.JwtToken, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return QueryResult<List<CurrencyEntity>>.Failure([new Error("Вы не авторизованы.")]);
        }
        
        var result = await currencyStorage.GetCurrenciesByCodesAsync(response.Content!, cancellationToken);
        
        return QueryResult<List<CurrencyEntity>>.Success(result);
    }
}