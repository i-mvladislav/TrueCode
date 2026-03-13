using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Domain.Dao;

public interface IUserStorage
{
    Task AddUserAsync(UserEntity user);
    Task<bool> HasUserAsync(string username);
    Task<UserEntity?> GetUserByIdAsync(Guid userId);
    Task<UserEntity?> GetUserByNameAsync(string username);
}