using Microsoft.EntityFrameworkCore;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Domain.Entities;
using TrueCode.UserService.Infrastructure.Data;

namespace TrueCode.UserService.Infrastructure.Dao;

public sealed class UserStorage(ApplicationContext db) : IUserStorage
{
    public async Task AddUserAsync(UserEntity user)
    {
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
    }

    public async Task<bool> HasUserAsync(string username)
    {
        var result = await db.Users.AnyAsync(u => u.Name == username);
        return result;
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid userId)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }

    public async Task<UserEntity?> GetUserByNameAsync(string username)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Name == username);
        return user;
    }
}