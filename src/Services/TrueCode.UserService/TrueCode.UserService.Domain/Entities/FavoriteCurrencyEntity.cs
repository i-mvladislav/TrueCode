using TrueCode.Core.Entities;

namespace TrueCode.UserService.Domain.Entities;

public class FavoriteCurrencyEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public string Code { get; set; } = null!;
}