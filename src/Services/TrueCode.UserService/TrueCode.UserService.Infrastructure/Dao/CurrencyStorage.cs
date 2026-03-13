using Microsoft.EntityFrameworkCore;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Domain.Entities;
using TrueCode.UserService.Infrastructure.Data;

namespace TrueCode.UserService.Infrastructure.Dao;

public sealed class CurrencyStorage(ApplicationContext db) : ICurrencyStorage
{
    public async Task AddFavoriteCurrencyAsync(FavoriteCurrencyEntity entity, CancellationToken cancellationToken = default)
    {
        await db.FavouriteCurrencies.AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<FavoriteCurrencyEntity>> GetFavoriteCurrenciesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var favoriteCurrencies = await db.FavouriteCurrencies
            .AsNoTracking()
            .Where(fc => fc.UserId == userId)
            .ToListAsync(cancellationToken);

        return favoriteCurrencies;
    }

    public async Task RemoveFavoriteCurrencyAsync(Guid userId, string currencyCode, CancellationToken cancellationToken = default)
    {
        await db.FavouriteCurrencies.Where(fc => fc.UserId == userId).ExecuteDeleteAsync(cancellationToken);
    }
}