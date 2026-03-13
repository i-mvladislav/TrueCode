using TrueCode.Core.Models;
using TrueCode.Core.Queries;
using TrueCode.UserService.HttpClients;

namespace TrueCode.FinanceService.Application.Currencies.Queries.GetCurrenciesByUser;

public class GetCurrenciesByUserQueryHandler(ICurrenciesHttpClient client) : BaseQueryHandler<GetCurrenciesByUserQuery, List<string>>
{
    protected override async Task<QueryResult<List<string>>> ExecuteCoreAsync(GetCurrenciesByUserQuery query, CancellationToken cancellationToken = default)
    {
        List<Error> errors = [];
        
        if (query.UserId == Guid.Empty)
            errors.Add(new Error("Некорректный пользователь."));
        
        if (string.IsNullOrWhiteSpace(query.JwtToken))
            errors.Add(new Error("Некорректный токен."));
        
        if (errors.Count > 0)
            return QueryResult<List<string>>.Failure(errors);
        
        var result = await client.GetFavoriteCurrenciesAsync(query.UserId, query.JwtToken, cancellationToken);
        
        return QueryResult<List<string>>.Success(result);
    }
}