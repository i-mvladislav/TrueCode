using TrueCode.Core.Models;
using TrueCode.Core.Queries;
using TrueCode.Core.Users;
using TrueCode.UserService.Domain.Dao;

namespace TrueCode.UserService.Application.Currencies.Queries;

internal class GetFavoriteCurrenciesQueryHandler(ICurrencyStorage currencyStorage, ICurrentUserContext userContext) : BaseQueryHandler<GetFavoriteCurrenciesQuery, List<string>>
{
    protected override async Task<QueryResult<List<string>>> ExecuteCoreAsync(GetFavoriteCurrenciesQuery command, CancellationToken cancellationToken = default)
    {
        List<Error> errors = [];

        if (!Guid.TryParse(userContext.UserId, out var userId) || userId == Guid.Empty)
            errors.Add(new Error("Некорректный пользователь."));

        if (errors.Count > 0)
            return QueryResult<List<string>>.Failure(errors);

        var favoriteCurrencies = await currencyStorage.GetFavoriteCurrenciesAsync(userId, cancellationToken);
        var result = favoriteCurrencies.Select(c => c.Code).ToList();

        return QueryResult<List<string>>.Success(result);
    }
}