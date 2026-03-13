namespace TrueCode.UserService.Application.Auth.Commands.SignIn;

public sealed record SignInCommand
{
    public required string Name { get; init; }
    public required string Password { get; init; }
}