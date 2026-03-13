using TrueCode.Core.Commands;
using TrueCode.Core.Models;
using TrueCode.UserService.Application.Auth.Models;
using TrueCode.UserService.Application.Contracts;
using TrueCode.UserService.Domain.Dao;

namespace TrueCode.UserService.Application.Auth.Commands.SignIn;

internal sealed class SignInCommandHandler(IUserStorage userStorage, IJwtService jwtService) : BaseCommandHandler<SignInCommand, JwtToken>
{
    protected override async Task<CommandResult<JwtToken>> ExecuteCoreAsync(SignInCommand request, CancellationToken ct = default)
    {
        List<Error> errors = [];
        
        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add(new Error("Имя пользователя не может быть пустым."));
        
        if (string.IsNullOrWhiteSpace(request.Password))
            errors.Add(new Error("Пароль не может быть пустым."));

        var userEntity = await userStorage.GetUserByNameAsync(request.Name);

        if (userEntity is null || !BCrypt.Net.BCrypt.Verify(request.Password, userEntity.PasswordHash))
            errors.Add(new Error("Неверное имя или пароль."));
        
        if (errors.Count > 0)
            return CommandResult<JwtToken>.Failure(errors);

        var result = jwtService.GenerateToken(userEntity!);
        
        return CommandResult<JwtToken>.Success(result);
    }
}