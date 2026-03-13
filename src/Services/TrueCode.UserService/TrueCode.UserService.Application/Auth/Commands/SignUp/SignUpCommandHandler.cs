using TrueCode.Core.Commands;
using TrueCode.Core.Models;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Application.Auth.Commands.SignUp;

internal sealed class SignUpCommandHandler(IUserStorage userStorage) : BaseCommandHandler<SignUpCommand>
{
    protected override async Task<CommandResult> ExecuteCoreAsync(SignUpCommand request, CancellationToken ct = default)
    {
        List<Error> errors = [];
        
        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add(new Error("Имя пользователя не может быть пустым."));
        
        if (string.IsNullOrWhiteSpace(request.Password))
            errors.Add(new Error("Пароль не может быть пустым."));
        
        if (await userStorage.HasUserAsync(request.Name))
            errors.Add(new Error("Пользователь с таким именем уже существует."));
        
        if (errors.Count > 0)
            return CommandResult.Failure(errors);
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)!;
        var user = new UserEntity
        {
            Name = request.Name,
            PasswordHash = passwordHash,
        };

        await userStorage.AddUserAsync(user);
        
        return CommandResult.Success();
    }
}