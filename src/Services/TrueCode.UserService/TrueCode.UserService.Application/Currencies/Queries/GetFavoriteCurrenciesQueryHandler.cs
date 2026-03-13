using TrueCode.Core.Models;
using TrueCode.Core.Queries;
using TrueCode.UserService.Domain.Dao;

namespace TrueCode.UserService.Application.Currencies.Queries;

public class GetFavoriteCurrenciesQueryHandler(ICurrencyStorage currencyStorage) : BaseQueryHandler<GetFavoriteCurrenciesQuery, List<string>>
{
    protected override async Task<QueryResult<List<string>>> ExecuteCoreAsync(GetFavoriteCurrenciesQuery command, CancellationToken cancellationToken = default)
    {
        List<Error> errors = [];

        if (command.UserId == Guid.Empty)
            errors.Add(new Error("Некорректный пользователь."));

        if (errors.Count > 0)
            return QueryResult<List<string>>.Failure(errors);

        var favoriteCurrencies = await currencyStorage.GetFavoriteCurrenciesAsync(command.UserId, cancellationToken);
        var result = favoriteCurrencies.Select(c => c.Code).ToList();

        return QueryResult<List<string>>.Success(result);
    }
}