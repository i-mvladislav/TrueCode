using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Domain.Dao;

public interface ICurrencyStorage
{
    public Task AddFavoriteCurrencyAsync(FavoriteCurrencyEntity entity, CancellationToken cancellationToken = default);
    public Task<List<FavoriteCurrencyEntity>> GetFavoriteCurrenciesAsync(Guid userId, CancellationToken cancellationToken = default);
    public Task RemoveFavoriteCurrencyAsync(Guid userId, string currencyCode, CancellationToken cancellationToken = default);
}