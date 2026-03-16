using System.Net;
using TrueCode.Core.Enums;
using TrueCode.Core.Models;
using TrueCode.Core.Queries;
using TrueCode.Core.Users;
using TrueCode.FinanceService.Application.Currencies.Models;
using TrueCode.FinanceService.Domain.Dao;
using TrueCode.UserService.HttpClients;

namespace TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesByUser;

internal class GetCurrenciesByUserQueryHandler(
    ICurrenciesHttpClient client,
    ICurrencyStorage currencyStorage,
    ICurrentUserContext userContext) : BaseQueryHandler<GetCurrenciesByUserQuery, List<Currency>>
{
    protected override async Task<QueryResult<List<Currency>>> ExecuteCoreAsync(GetCurrenciesByUserQuery query, CancellationToken cancellationToken = default)
    {
        var jwtToken = !string.IsNullOrWhiteSpace(userContext.Authorization)
            ? userContext.Authorization?["Bearer ".Length..]
            : null;
        List<Error> errors = [];
        
        if (!Guid.TryParse(userContext.UserId, out var userId) || userId == Guid.Empty)
            errors.Add(new Error("Некорректный пользователь."));
        
        if (string.IsNullOrWhiteSpace(jwtToken))
            errors.Add(new Error("Некорректный токен."));
        
        if (errors.Count > 0)
            return QueryResult<List<Currency>>.Failure(errors);
        
        var response = await client.GetFavoriteCurrenciesAsync(userId, jwtToken, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return QueryResult<List<Currency>>.Failure([new Error("Вы не авторизованы.", ErrorType.Unauthorized)]);
        }
        
        var currenciesEntities = await currencyStorage.GetCurrenciesByCodesAsync(response.Content!, cancellationToken);
        var result = currenciesEntities.Select(ce => new Currency
        {
            Id = ce.Id,
            Name = ce.Name,
            Rate = ce.Rate,
        }).ToList();
        
        return QueryResult<List<Currency>>.Success(result);
    }
}