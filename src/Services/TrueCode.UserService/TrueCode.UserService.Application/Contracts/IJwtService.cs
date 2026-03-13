using TrueCode.UserService.Application.Auth.Models;
using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Application.Contracts;

public interface IJwtService
{
    JwtToken GenerateToken(UserEntity user);
}