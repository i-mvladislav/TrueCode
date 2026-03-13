using TrueCode.Core.Entities;

namespace TrueCode.UserService.Domain.Entities;

public class UserEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public List<FavoriteCurrencyEntity> FavouriteCurrencies { get; set; } = [];
}